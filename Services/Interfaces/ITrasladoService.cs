using TransportesGenesis.DTOs.Traslado;

namespace TransportesGenesis.Services.Interfaces
{
    public interface ITrasladoService
    {
        // Solicitudes
        Task<SolicitudTrasladoDto> CrearSolicitudAsync(CrearSolicitudTrasladoDto dto);
        Task<SolicitudTrasladoDto?> GetSolicitudPorIdAsync(int idSolicitud);
        Task<IEnumerable<SolicitudTrasladoDto>> GetSolicitudesPorAlumnoAsync(int idAlumno);
        Task<IEnumerable<SolicitudTrasladoDto>> GetSolicitudesPendientesAsync();

        // Aprobación/Rechazo (Admin)
        Task<SolicitudTrasladoDto> ResponderSolicitudAsync(ResponderSolicitudTrasladoDto dto, string usuarioAdmin);

        // Consultas
        Task<IEnumerable<BusDisponibleDto>> GetBusesDisponiblesAsync(DateTime fecha, string turno);
        Task<SolicitudTrasladoDto?> GetTrasladoActivoAsync(int idAlumno, DateTime fecha);
        Task<IEnumerable<SolicitudTrasladoDto>> GetTrasladosDelDiaAsync(DateTime fecha);
    }
}
