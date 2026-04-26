using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Repositories.Interfaces
{
    public interface IUbicacionBusRepository : IRepositoryBase<UbicacionBusEnTiempoReal>
    {
        Task<UbicacionBusEnTiempoReal?> GetUltimaUbicacionAsync(int idBus);
        Task<IEnumerable<UbicacionBusEnTiempoReal>> GetHistorialAsync(int idBus, DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<UbicacionBusEnTiempoReal>> GetUltimasUbicacionesBusesActivosAsync();
        Task<bool> LimpiarHistorialAntiguoAsync(DateTime fechaLimite);
    }
}
