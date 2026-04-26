using AutoMapper;
using TransportesGenesis.DTOs.Traslado;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Services.Implementations
{
    public class TrasladoService : ITrasladoService
    {
        private readonly ISolicitudTrasladoRepository _solicitudRepository;
        private readonly IBusRepository _busRepository;
        private readonly IAsistenciaAlumnoRepository _asistenciaRepository;
        private readonly IMapper _mapper;

        public TrasladoService(
            ISolicitudTrasladoRepository solicitudRepository,
            IBusRepository busRepository,
            IAsistenciaAlumnoRepository asistenciaRepository,
            IMapper mapper)
        {
            _solicitudRepository = solicitudRepository;
            _busRepository = busRepository;
            _asistenciaRepository = asistenciaRepository;
            _mapper = mapper;
        }

        public async Task<SolicitudTrasladoDto> CrearSolicitudAsync(CrearSolicitudTrasladoDto dto)
        {
            // Validar que no haya otra solicitud activa para esa fecha
            var solicitudExistente = await _solicitudRepository.GetSolicitudActivaAsync(dto.IdAlumno, dto.FechaTraslado);
            if (solicitudExistente != null)
                throw new InvalidOperationException("Ya existe una solicitud aprobada para esta fecha");

            // Validar que la fecha no sea pasada
            if (dto.FechaTraslado.Date < DateTime.Today)
                throw new InvalidOperationException("No se pueden crear solicitudes para fechas pasadas");

            var solicitud = _mapper.Map<SolicitudTraslado>(dto);
            solicitud.Estado = "Pendiente";
            solicitud.FechaRegistro = DateTime.Now; // ✅ FIX: Establecer fecha de registro
            solicitud.Activo = 1; // ✅ FIX: Marcar como activo

            // Obtener el bus origen del alumno
            // IdBusOrigen ya viene del dto si lo enviaron, si no, usar bus del alumno
            if (solicitud.IdBusOrigen == 0)
            {
                solicitud.IdBusOrigen = 4; // Bus por defecto (BUS-001)
            }

            var resultado = await _solicitudRepository.AddAsync(solicitud);
            return _mapper.Map<SolicitudTrasladoDto>(resultado);
        }

        public async Task<SolicitudTrasladoDto?> GetSolicitudPorIdAsync(int idSolicitud)
        {
            var solicitud = await _solicitudRepository.GetByIdAsync(idSolicitud);
            return solicitud == null ? null : _mapper.Map<SolicitudTrasladoDto>(solicitud);
        }

        public async Task<IEnumerable<SolicitudTrasladoDto>> GetSolicitudesPorAlumnoAsync(int idAlumno)
        {
            var solicitudes = await _solicitudRepository.GetByAlumnoAsync(idAlumno);
            return _mapper.Map<IEnumerable<SolicitudTrasladoDto>>(solicitudes);
        }

        public async Task<IEnumerable<SolicitudTrasladoDto>> GetSolicitudesPendientesAsync()
        {
            var solicitudes = await _solicitudRepository.GetPendientesAsync();
            return _mapper.Map<IEnumerable<SolicitudTrasladoDto>>(solicitudes);
        }

        public async Task<SolicitudTrasladoDto> ResponderSolicitudAsync(ResponderSolicitudTrasladoDto dto, string usuarioAdmin)
        {
            var solicitud = await _solicitudRepository.GetByIdAsync(dto.IdSolicitud);
            if (solicitud == null)
                throw new KeyNotFoundException($"Solicitud con ID {dto.IdSolicitud} no encontrada");

            if (solicitud.Estado != "Pendiente")
                throw new InvalidOperationException("Solo se pueden responder solicitudes pendientes");

            // Actualizar estado
            solicitud.Estado = dto.Estado;
            solicitud.AprobadoPor = usuarioAdmin;
            solicitud.FechaRespuesta = DateTime.Now;
            solicitud.ComentarioAdmin = dto.ComentarioAdmin;

            if (dto.Estado == "Aprobado")
            {
                if (!dto.IdBusDestino.HasValue)
                    throw new InvalidOperationException("Debe especificar el bus destino al aprobar la solicitud");

                solicitud.IdBusDestino = dto.IdBusDestino;

                // Actualizar la asistencia del día con el bus temporal
                await ActualizarAsistenciaConTrasladoAsync(solicitud);
            }

            await _solicitudRepository.UpdateAsync(solicitud);
            return _mapper.Map<SolicitudTrasladoDto>(solicitud);
        }

        public async Task<IEnumerable<BusDisponibleDto>> GetBusesDisponiblesAsync(DateTime fecha, string turno)
        {
            var buses = await _busRepository.GetAllAsync();
            var busesActivos = buses.Where(b => b.Estado == true).ToList();

            var busesDisponibles = new List<BusDisponibleDto>();

            foreach (var bus in busesActivos)
            {
                // Calcular asientos disponibles
                // Por ahora retornamos capacidad - asientos ocupados (simplificado)
                busesDisponibles.Add(new BusDisponibleDto
                {
                    IdBus = bus.IdBus,
                    Placa = bus.Placa,
                    Modelo = bus.Modelo,
                    Capacidad = bus.Capacidad,
                    AsientosDisponibles = bus.Capacidad // TODO: calcular ocupación real
                });
            }

            return busesDisponibles.OrderBy(b => b.Placa);
        }

        public async Task<SolicitudTrasladoDto?> GetTrasladoActivoAsync(int idAlumno, DateTime fecha)
        {
            var solicitud = await _solicitudRepository.GetSolicitudActivaAsync(idAlumno, fecha);
            return solicitud == null ? null : _mapper.Map<SolicitudTrasladoDto>(solicitud);
        }

        public async Task<IEnumerable<SolicitudTrasladoDto>> GetTrasladosDelDiaAsync(DateTime fecha)
        {
            var solicitudes = await _solicitudRepository.GetSolicitudesPorFechaAsync(fecha);
            return _mapper.Map<IEnumerable<SolicitudTrasladoDto>>(solicitudes);
        }

        private async Task ActualizarAsistenciaConTrasladoAsync(SolicitudTraslado solicitud)
        {
            var asistencia = await _asistenciaRepository.GetByAlumnoYFechaAsync(
                solicitud.IdAlumno, 
                solicitud.FechaTraslado);

            if (asistencia == null)
            {
                // Crear asistencia si no existe
                asistencia = new AsistenciaAlumno
                {
                    IdAlumno = solicitud.IdAlumno,
                    Fecha = solicitud.FechaTraslado,
                    AsisteMañana = true,
                    AsisteTarde = true
                };
                asistencia = await _asistenciaRepository.AddAsync(asistencia);
            }

            // Actualizar bus temporal según el turno
            if (solicitud.Turno == "Mañana" || solicitud.Turno == "Ambos")
                asistencia.IdBusTemporalMañana = solicitud.IdBusDestino;

            if (solicitud.Turno == "Tarde" || solicitud.Turno == "Ambos")
                asistencia.IdBusTemporalTarde = solicitud.IdBusDestino;

            await _asistenciaRepository.UpdateAsync(asistencia);
        }
    }
}
