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
        private readonly IMapper _mapper;

        public BusService(IBusRepository busRepository, IMapper mapper)
        {
            _busRepository = busRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BusDto>> GetAllBusesAsync()
        {
            var buses = await _busRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BusDto>>(buses);
        }

        public async Task<IEnumerable<BusDto>> GetBusesActivosAsync()
        {
            var buses = await _busRepository.GetActivosAsync();
            return _mapper.Map<IEnumerable<BusDto>>(buses);
        }

        public async Task<BusDto?> GetBusByIdAsync(int idBus)
        {
            var bus = await _busRepository.GetByIdAsync(idBus);
            return bus == null ? null : _mapper.Map<BusDto>(bus);
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
    }
}
