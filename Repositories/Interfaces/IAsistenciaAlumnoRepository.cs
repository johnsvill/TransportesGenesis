using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Repositories.Interfaces
{
    public interface IAsistenciaAlumnoRepository : IRepositoryBase<AsistenciaAlumno>
    {
        Task<AsistenciaAlumno?> GetByAlumnoYFechaAsync(int idAlumno, DateTime fecha);
        Task<IEnumerable<AsistenciaAlumno>> GetByAlumnoYRangoAsync(int idAlumno, DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<AsistenciaAlumno>> GetConfirmadasDelDiaAsync(DateTime fecha, bool asisteMañana, bool asisteTarde);
        Task<bool> ConfirmarAsistenciaAsync(int idAsistencia);
        Task<IEnumerable<AsistenciaAlumno>> GetPendientesDeConfirmarAsync(DateTime fecha);
        Task<IEnumerable<AsistenciaAlumno>> GetConfirmacionesPorFechaAsync(DateTime fecha);
    }
}
