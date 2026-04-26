using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Repositories.Interfaces
{
    public interface ISolicitudTrasladoRepository : IRepositoryBase<SolicitudTraslado>
    {
        Task<IEnumerable<SolicitudTraslado>> GetByAlumnoAsync(int idAlumno);
        Task<IEnumerable<SolicitudTraslado>> GetPendientesAsync();
        Task<IEnumerable<SolicitudTraslado>> GetByEstadoAsync(string estado);
        Task<bool> AprobarAsync(int idSolicitud, string idAdmin, int idBusDestino, string? comentario);
        Task<bool> RechazarAsync(int idSolicitud, string idAdmin, string? comentario);

        // Métodos adicionales para el servicio
        Task<IEnumerable<SolicitudTraslado>> GetSolicitudesPorFechaAsync(DateTime fecha);
        Task<SolicitudTraslado?> GetSolicitudActivaAsync(int idAlumno, DateTime fecha);
    }
}
