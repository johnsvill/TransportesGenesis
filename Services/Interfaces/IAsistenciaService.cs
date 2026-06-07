using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.DTOs.Asistencia;

namespace TransportesGenesis.Services.Interfaces
{
    public interface IAsistenciaService
    {
        Task<AsistenciaAlumnoDto?> GetAsistenciaDelDiaAsync(int idAlumno, DateTime fecha);
        Task<AsistenciaCalendarioDto> GetCalendarioMensualAsync(int idAlumno, int mes, int anio);
        Task<AsistenciaAlumnoDto> RegistrarAsistenciaAsync(AsistenciaCreateDto dto);
        Task<AsistenciaAlumnoDto> ActualizarAsistenciaAsync(AsistenciaUpdateDto dto);
        Task<bool> ConfirmarAsistenciaAsync(int idAsistencia);
        Task<IEnumerable<AsistenciaAlumnoDto>> GetAsistenciasDelDiaAsync(DateTime fecha);
        Task<bool> ProcesarConfirmacionesAutomaticasAsync(DateTime fecha, string turno);

        // FASE 4: Nuevos métodos para confirmación de padres
        Task<AsistenciaResumenDto?> GetResumenAsistenciaHoyAsync(int idAlumno);
        Task<IEnumerable<AsistenciaResumenDto>> GetResumenAsistenciasPorPadreAsync(int idPadre);
        Task<AsistenciaDto?> ConfirmarAsistenciaParaHoyAsync(ConfirmarAsistenciaDto dto);
        Task<bool> PuedeConfirmarMañanaAsync(DateTime fechaActual);
        Task<bool> PuedeConfirmarTardeAsync(DateTime fechaActual);
        Task<IEnumerable<AsistenciaDto>> GetHistorialAsistenciaAsync(int idAlumno, DateTime fechaInicio, DateTime fechaFin);
    }
}
