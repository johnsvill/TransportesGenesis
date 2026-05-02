using TransportesGenesis.DTOs.Geolocalizacion;

namespace TransportesGenesis.Services.Interfaces
{
    public interface IBusService
    {
        Task<IEnumerable<BusDto>> GetAllBusesAsync();
        Task<IEnumerable<BusDto>> GetBusesActivosAsync();
        Task<BusDto?> GetBusByIdAsync(int idBus);
        Task<BusDto?> GetBusByPlacaAsync(string placa);
        Task<BusDto> CreateBusAsync(BusCreateDto dto);
        Task<BusDto> UpdateBusAsync(BusUpdateDto dto);
        Task<bool> DeleteBusAsync(int idBus);
        Task<bool> ActivarDesactivarBusAsync(int idBus, bool activar);
    }
}
