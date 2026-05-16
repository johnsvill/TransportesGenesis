using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Services.Interfaces
{
    public interface IPilotoService
    {
        Task<AsignacionPilotoBus?> GetAsignacionActualAsync(string idUsuarioPiloto);
        Task<int?> GetIdBusAsignadoAsync(string idUsuarioPiloto);
    }
}
