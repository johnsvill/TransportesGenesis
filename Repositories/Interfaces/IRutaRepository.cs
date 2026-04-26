using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Repositories.Interfaces
{
    public interface IRutaRepository : IRepositoryBase<Ruta>
    {
        Task<IEnumerable<Ruta>> GetByBusAsync(int idBus);
        Task<IEnumerable<Ruta>> GetActivasByBusAsync(int idBus);
        Task<Ruta?> GetConParadasAsync(int idRuta);
        Task<IEnumerable<Ruta>> GetByTipoAsync(string tipoRuta);
        Task<IEnumerable<Ruta>> GetRutasDelDiaAsync(DateTime fecha, string tipoRuta);
        Task<Parada?> GetParadaByIdAsync(int idParada);
        Task<bool> MarcarParadaCompletadaAsync(int idParada, bool completada);
    }
}
