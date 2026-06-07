using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Repositories.Interfaces
{
    public interface IBusRepository : IRepositoryBase<Bus>
    {
        Task<IEnumerable<Bus>> GetActivosAsync();
        Task<Bus?> GetByPlacaAsync(string placa);
        Task<Bus?> GetConRutasAsync(int idBus);
        Task<IEnumerable<Bus>> GetConRutasActivasAsync();
    }
}
