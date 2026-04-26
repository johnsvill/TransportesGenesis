using AutoMapper;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Services.Implementations
{
    public class UbicacionBusService : IUbicacionBusService
    {
        private readonly IUbicacionBusRepository _ubicacionRepository;
        private readonly IBusRepository _busRepository;
        private readonly IMapper _mapper;

        public UbicacionBusService(
            IUbicacionBusRepository ubicacionRepository,
            IBusRepository busRepository,
            IMapper mapper)
        {
            _ubicacionRepository = ubicacionRepository;
            _busRepository = busRepository;
            _mapper = mapper;
        }

        public async Task<UbicacionBusDto?> GetUltimaUbicacionAsync(int idBus)
        {
            var ubicacion = await _ubicacionRepository.GetUltimaUbicacionAsync(idBus);
            return ubicacion == null ? null : _mapper.Map<UbicacionBusDto>(ubicacion);
        }

        public async Task<IEnumerable<UbicacionBusEnMapaDto>> GetUbicacionesBusesActivosAsync()
        {
            var ubicaciones = await _ubicacionRepository.GetUltimasUbicacionesBusesActivosAsync();
            var dtos = new List<UbicacionBusEnMapaDto>();

            foreach (var ubicacion in ubicaciones)
            {
                var dto = _mapper.Map<UbicacionBusEnMapaDto>(ubicacion);

                // Calcular estado basado en última actualización
                var minutosDesdeActualizacion = (DateTime.Now - ubicacion.FechaHora).TotalMinutes;

                if (minutosDesdeActualizacion > 10)
                    dto.Estado = "SinSeñal";
                else if (ubicacion.Velocidad.HasValue && ubicacion.Velocidad > 5)
                    dto.Estado = "Movimiento";
                else
                    dto.Estado = "Detenido";

                // Por ahora, alumnos pendientes = 0 (se calculará con ParadaRepository más adelante)
                dto.AlumnosPendientes = 0;

                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<UbicacionBusDto> RegistrarUbicacionAsync(UbicacionBusCreateDto dto)
        {
            // Validar que el bus existe
            var busExiste = await _busRepository.ExistsAsync(dto.IdBus);
            if (!busExiste)
                throw new KeyNotFoundException($"Bus con ID {dto.IdBus} no encontrado");

            var ubicacion = _mapper.Map<UbicacionBusEnTiempoReal>(dto);
            var ubicacionCreada = await _ubicacionRepository.AddAsync(ubicacion);

            return _mapper.Map<UbicacionBusDto>(ubicacionCreada);
        }

        public async Task<IEnumerable<UbicacionBusDto>> GetHistorialAsync(int idBus, DateTime fechaInicio, DateTime fechaFin)
        {
            var historial = await _ubicacionRepository.GetHistorialAsync(idBus, fechaInicio, fechaFin);
            return _mapper.Map<IEnumerable<UbicacionBusDto>>(historial);
        }

        public async Task<bool> LimpiarHistorialAntiguoAsync(int diasAntiguedad)
        {
            var fechaLimite = DateTime.Now.AddDays(-diasAntiguedad);
            return await _ubicacionRepository.LimpiarHistorialAntiguoAsync(fechaLimite);
        }
    }
}
