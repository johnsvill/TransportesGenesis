using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Repositories.Interfaces
{
    public interface IParadaRepository : IRepositoryBase<Parada>
    {
        Task<IEnumerable<Parada>> GetByRutaAsync(int idRuta);
        Task<IEnumerable<Parada>> GetPendientesByRutaAsync(int idRuta);
        Task<Parada?> GetByAlumnoYRutaAsync(int idAlumno, int idRuta);
        Task<bool> ActualizarOrdenAsync(int idRuta, List<int> idsParadasEnOrden);
        Task<bool> MarcarCompletadaAsync(int idParada);
    }
}
