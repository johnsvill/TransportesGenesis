using AutoMapper;
using TransportesGenesis.DTOs.Asistencia;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Services.Implementations
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IAsistenciaAlumnoRepository _asistenciaRepository;
        private readonly IBusRepository _busRepository;
        private readonly IMapper _mapper;

        public AsistenciaService(
            IAsistenciaAlumnoRepository asistenciaRepository,
            IBusRepository busRepository,
            IMapper mapper)
        {
            _asistenciaRepository = asistenciaRepository;
            _busRepository = busRepository;
            _mapper = mapper;
        }

        #region Métodos Existentes (mantener compatibilidad)

        public async Task<AsistenciaAlumnoDto?> GetAsistenciaDelDiaAsync(int idAlumno, DateTime fecha)
        {
            var asistencia = await _asistenciaRepository.GetByAlumnoYFechaAsync(idAlumno, fecha);
            return asistencia == null ? null : _mapper.Map<AsistenciaAlumnoDto>(asistencia);
        }

        public async Task<AsistenciaCalendarioDto> GetCalendarioMensualAsync(int idAlumno, int mes, int anio)
        {
            var primerDia = new DateTime(anio, mes, 1);
            var ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            var asistencias = await _asistenciaRepository.GetByAlumnoYRangoAsync(idAlumno, primerDia, ultimoDia);

            var calendario = new AsistenciaCalendarioDto
            {
                IdAlumno = idAlumno,
                NombreAlumno = asistencias.FirstOrDefault()?.Alumno != null 
                    ? $"{asistencias.FirstOrDefault().Alumno.Nombre} {asistencias.FirstOrDefault().Alumno.Apellido}"
                    : "",
                Calendario = asistencias.ToDictionary(
                    a => a.Fecha.Date,
                    a => new AsistenciaDiaDto
                    {
                        AsisteMañana = a.AsisteMañana,
                        AsisteTarde = a.AsisteTarde,
                        Confirmado = a.FechaConfirmacion.HasValue
                    }
                )
            };

            return calendario;
        }

        public async Task<AsistenciaAlumnoDto> RegistrarAsistenciaAsync(AsistenciaCreateDto dto)
        {
            var asistencia = _mapper.Map<AsistenciaAlumno>(dto);
            asistencia.Fecha = dto.Fecha.Date; // Solo fecha, sin hora

            var resultado = await _asistenciaRepository.AddAsync(asistencia);
            return _mapper.Map<AsistenciaAlumnoDto>(resultado);
        }

        public async Task<AsistenciaAlumnoDto> ActualizarAsistenciaAsync(AsistenciaUpdateDto dto)
        {
            var asistencia = await _asistenciaRepository.GetByIdAsync(dto.IdAsistencia);
            if (asistencia == null)
                throw new KeyNotFoundException($"Asistencia con ID {dto.IdAsistencia} no encontrada");

            _mapper.Map(dto, asistencia);
            await _asistenciaRepository.UpdateAsync(asistencia);

            return _mapper.Map<AsistenciaAlumnoDto>(asistencia);
        }

        public async Task<bool> ConfirmarAsistenciaAsync(int idAsistencia)
        {
            return await _asistenciaRepository.ConfirmarAsistenciaAsync(idAsistencia);
        }

        public async Task<IEnumerable<AsistenciaAlumnoDto>> GetAsistenciasDelDiaAsync(DateTime fecha)
        {
            var asistencias = await _asistenciaRepository.GetConfirmadasDelDiaAsync(fecha, true, true);
            return _mapper.Map<IEnumerable<AsistenciaAlumnoDto>>(asistencias);
        }

        public async Task<bool> ProcesarConfirmacionesAutomaticasAsync(DateTime fecha, string turno)
        {
            // Lógica para marcar como no asistentes a los que no confirmaron
            var pendientes = await _asistenciaRepository.GetPendientesDeConfirmarAsync(fecha);

            foreach (var asistencia in pendientes)
            {
                if (turno.ToLower() == "mañana")
                    asistencia.AsisteMañana = false;
                else if (turno.ToLower() == "tarde")
                    asistencia.AsisteTarde = false;

                asistencia.FechaConfirmacion = DateTime.Now;
                await _asistenciaRepository.UpdateAsync(asistencia);
            }

            return true;
        }

        #endregion

        #region FASE 4: Métodos para Confirmación de Padres

        public async Task<AsistenciaResumenDto?> GetResumenAsistenciaHoyAsync(int idAlumno)
        {
            var hoy = DateTime.Today;
            var asistencia = await _asistenciaRepository.GetByAlumnoYFechaAsync(idAlumno, hoy);

            if (asistencia == null)
            {
                // Crear asistencia para hoy si no existe
                var nuevaAsistencia = new AsistenciaAlumno
                {
                    Alumno = new Alumnos { IdAlumno = idAlumno },
                    Fecha = hoy,
                    AsisteMañana = true,
                    AsisteTarde = true,
                    FechaConfirmacion = null
                };
                asistencia = await _asistenciaRepository.AddAsync(nuevaAsistencia);
            }

            var ahora = DateTime.Now;
            var puedeConfirmarMañana = await PuedeConfirmarMañanaAsync(ahora);
            var puedeConfirmarTarde = await PuedeConfirmarTardeAsync(ahora);

            var bus = asistencia.Alumno.IdBusAsignado.HasValue
                ? await _busRepository.GetByIdAsync(asistencia.Alumno.IdBusAsignado.Value)
                : null;

            var resumen = new AsistenciaResumenDto
            {
                IdAlumno = asistencia.Alumno.IdAlumno,
                NombreCompleto = $"{asistencia.Alumno.Nombre} {asistencia.Alumno.Apellido}",
                IdBusAsignado = asistencia.Alumno.IdBusAsignado ?? 0,
                PlacaBus = bus?.Placa ?? "Sin asignar",
                TieneConfirmacionHoy = asistencia.FechaConfirmacion.HasValue,
                AsisteMañanaHoy = asistencia.AsisteMañana,
                AsisteTardeHoy = asistencia.AsisteTarde,
                FechaUltimaConfirmacion = asistencia.FechaConfirmacion,
                PuedeConfirmarMañana = puedeConfirmarMañana,
                PuedeConfirmarTarde = puedeConfirmarTarde,
                MensajeEstado = ObtenerMensajeEstado(asistencia, puedeConfirmarMañana, puedeConfirmarTarde)
            };

            return resumen;
        }

        public async Task<IEnumerable<AsistenciaResumenDto>> GetResumenAsistenciasPorPadreAsync(int idPadre)
        {
            // Por implementar: obtener todos los alumnos del padre y sus resúmenes
            var resumenes = new List<AsistenciaResumenDto>();
            // TODO: Implementar cuando tengas el repositorio de alumnos por padre
            return resumenes;
        }

        public async Task<AsistenciaDto?> ConfirmarAsistenciaParaHoyAsync(ConfirmarAsistenciaDto dto)
        {
            var ahora = DateTime.Now;
            var hoy = DateTime.Today;

            // Validar que sea para hoy
            if (dto.Fecha.Date != hoy)
                throw new InvalidOperationException("Solo se puede confirmar la asistencia para el día de hoy");

            // Validar horarios permitidos
            var puedeConfirmarMañana = await PuedeConfirmarMañanaAsync(ahora);
            var puedeConfirmarTarde = await PuedeConfirmarTardeAsync(ahora);

            if (dto.AsisteMañana && !puedeConfirmarMañana)
                throw new InvalidOperationException("Ya no es posible confirmar asistencia para la mañana. El límite es hasta las 4:00 AM");

            if (dto.AsisteTarde && !puedeConfirmarTarde)
                throw new InvalidOperationException("Ya no es posible confirmar asistencia para la tarde. El límite es hasta las 11:00 AM");

            // Buscar o crear asistencia
            var asistencia = await _asistenciaRepository.GetByAlumnoYFechaAsync(dto.IdAlumno, hoy);

            if (asistencia == null)
            {
                asistencia = new AsistenciaAlumno
                {
                    Alumno = new Alumnos { IdAlumno = dto.IdAlumno },
                    Fecha = hoy
                };
            }

            // Actualizar valores
            asistencia.AsisteMañana = dto.AsisteMañana;
            asistencia.AsisteTarde = dto.AsisteTarde;
            asistencia.IdBusTemporalMañana = dto.IdBusTemporalMañana;
            asistencia.IdBusTemporalTarde = dto.IdBusTemporalTarde;
            asistencia.FechaConfirmacion = ahora;

            if (asistencia.IdAsistencia == 0)
                asistencia = await _asistenciaRepository.AddAsync(asistencia);
            else
                await _asistenciaRepository.UpdateAsync(asistencia);

            return _mapper.Map<AsistenciaDto>(asistencia);
        }

        public Task<bool> PuedeConfirmarMañanaAsync(DateTime fechaActual)
        {
            // Se puede confirmar para la mañana hasta las 4:00 AM del mismo día
            var limiteConfirmacion = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 4, 0, 0);
            return Task.FromResult(fechaActual <= limiteConfirmacion);
        }

        public Task<bool> PuedeConfirmarTardeAsync(DateTime fechaActual)
        {
            // Se puede confirmar para la tarde hasta las 11:00 AM del mismo día
            var limiteConfirmacion = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 11, 0, 0);
            return Task.FromResult(fechaActual <= limiteConfirmacion);
        }

        public async Task<IEnumerable<AsistenciaDto>> GetHistorialAsistenciaAsync(int idAlumno, DateTime fechaInicio, DateTime fechaFin)
        {
            var asistencias = await _asistenciaRepository.GetByAlumnoYRangoAsync(idAlumno, fechaInicio, fechaFin);
            return _mapper.Map<IEnumerable<AsistenciaDto>>(asistencias);
        }

        #endregion

        #region Métodos Privados

        private string ObtenerMensajeEstado(AsistenciaAlumno asistencia, bool puedeConfirmarMañana, bool puedeConfirmarTarde)
        {
            if (asistencia.FechaConfirmacion.HasValue)
            {
                var confirmacion = asistencia.FechaConfirmacion.Value;
                return $"✅ Confirmado el {confirmacion:dd/MM/yyyy} a las {confirmacion:HH:mm}";
            }

            if (!puedeConfirmarMañana && !puedeConfirmarTarde)
                return "⚠️ Fuera de horario de confirmación";

            if (puedeConfirmarMañana)
                return $"⏰ Puede confirmar para la mañana hasta las 4:00 AM";

            if (puedeConfirmarTarde)
                return $"⏰ Puede confirmar para la tarde hasta las 11:00 AM";

            return "📋 Pendiente de confirmación";
        }

        #endregion
    }
}
