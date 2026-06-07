using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Repositories.Interfaces
{
    public interface IAlertaRepository : IRepositoryBase<Alerta>
    {
        Task<IEnumerable<Alerta>> GetByDestinatarioAsync(string idUsuario);
        Task<IEnumerable<Alerta>> GetNoLeidasByDestinatarioAsync(string idUsuario);
        Task<bool> MarcarComoLeidaAsync(int idAlerta);
        Task<bool> MarcarTodasComoLeidasAsync(string idUsuario);
        Task<IEnumerable<Alerta>> GetByTipoAsync(string tipoAlerta);
    }
}
