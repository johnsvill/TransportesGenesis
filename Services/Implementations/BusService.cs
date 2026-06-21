using AutoMapper;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Services.Implementations
{
    public class BusService : IBusService
    {
        private readonly IBusRepository _busRepository;
        private readonly IUbicacionBusService _ubicacionService;
        private readonly IMapper _mapper;

        public BusService(
            IBusRepository busRepository,
            IUbicacionBusService ubicacionService,
            IMapper mapper)
        {
            _busRepository = busRepository;
            _ubicacionService = ubicacionService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BusDto>> GetAllBusesAsync()
        {
            var buses = await _busRepository.GetAllConRutasAsync();
            var busesEnVivo = (await _ubicacionService.GetUbicacionesBusesActivosAsync())
                .Where(u => u.Estado != "SinSeñal")
                .Select(u => u.IdBus)
                .ToHashSet();

            return buses.Select(bus => MapBusDto(bus, busesEnVivo)).ToList();
        }

        public async Task<IEnumerable<BusDto>> GetBusesActivosAsync()
        {
            var buses = await _busRepository.GetAllConRutasAsync();
            var busesEnVivo = (await _ubicacionService.GetUbicacionesBusesActivosAsync())
                .Where(u => u.Estado != "SinSeñal")
                .Select(u => u.IdBus)
                .ToHashSet();

            return buses
                .Where(b => b.Estado)
                .Select(bus => MapBusDto(bus, busesEnVivo))
                .ToList();
        }

        public async Task<BusDto?> GetBusByIdAsync(int idBus)
        {
            var bus = await _busRepository.GetConRutasAsync(idBus);
            if (bus == null) return null;

            var busesEnVivo = (await _ubicacionService.GetUbicacionesBusesActivosAsync())
                .Where(u => u.Estado != "SinSeñal")
                .Select(u => u.IdBus)
                .ToHashSet();

            return MapBusDto(bus, busesEnVivo);
        }

        public async Task<BusDto?> GetBusByPlacaAsync(string placa)
        {
            var bus = await _busRepository.GetByPlacaAsync(placa);
            return bus == null ? null : _mapper.Map<BusDto>(bus);
        }

        public async Task<BusDto> CreateBusAsync(BusCreateDto dto)
        {
            var bus = _mapper.Map<Bus>(dto);
            bus.Estado = true;
            bus.FechaRegistro = DateTime.Now;
            bus.Activo = 1;

            var createdBus = await _busRepository.AddAsync(bus);
            return _mapper.Map<BusDto>(createdBus);
        }

        public async Task<BusDto> UpdateBusAsync(BusUpdateDto dto)
        {
            var busExistente = await _busRepository.GetByIdAsync(dto.IdBus);
            if (busExistente == null)
                throw new KeyNotFoundException($"Bus con ID {dto.IdBus} no encontrado");

            _mapper.Map(dto, busExistente);
            var updatedBus = await _busRepository.UpdateAsync(busExistente);
            return _mapper.Map<BusDto>(updatedBus);
        }

        public async Task<bool> DeleteBusAsync(int idBus)
        {
            return await _busRepository.DeleteAsync(idBus);
        }

        public async Task<bool> ActivarDesactivarBusAsync(int idBus, bool activar)
        {
            var bus = await _busRepository.GetByIdAsync(idBus);
            if (bus == null) return false;

            bus.Estado = activar;
            await _busRepository.UpdateAsync(bus);
            return true;
        }

        private static BusDto MapBusDto(Bus bus, HashSet<int> busesEnVivo)
        {
            var rutas = bus.RutasLink ?? new List<Ruta>();
            var tieneRutaOperativa = rutas.Any(r => r.EsActiva);
            var enRecorrido = tieneRutaOperativa && busesEnVivo.Contains(bus.IdBus);

            return new BusDto
            {
                IdBus = bus.IdBus,
                Placa = bus.Placa,
                Modelo = bus.Modelo,
                Capacidad = bus.Capacidad,
                Estado = bus.Estado,
                FechaRegistro = bus.FechaRegistro,
                RutasAsignadas = rutas.Count,
                RutasActivas = enRecorrido ? 1 : 0
            };
        }
    }
}
