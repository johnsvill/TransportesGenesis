using TransportesGenesis.DTOs.Notificaciones;

namespace TransportesGenesis.Services.Interfaces
{
    /// <summary>
    /// Servicio para gestionar alertas de proximidad y retrasos del sistema de transporte escolar
    /// </summary>
    public interface IAlertaService
    {
        /// <summary>
        /// Registra una nueva alerta de proximidad o retraso
        /// </summary>
        /// <param name="dto">Datos para crear la alerta</param>
        /// <returns>DTO de la alerta creada</returns>
        Task<AlertaProximidadDto> RegistrarAlertaAsync(AlertaProximidadCreateDto dto);

        /// <summary>
        /// Obtiene el historial de alertas por bus
        /// </summary>
        /// <param name="idBus">ID del bus</param>
        /// <param name="incluirResueltas">Incluir alertas resueltas</param>
        /// <returns>Lista de alertas del bus</returns>
        Task<IEnumerable<AlertaProximidadDto>> GetHistorialAlertasPorBusAsync(int idBus, bool incluirResueltas = false);

        /// <summary>
        /// Obtiene el historial de alertas por alumno
        /// </summary>
        /// <param name="idAlumno">ID del alumno</param>
        /// <param name="incluirResueltas">Incluir alertas resueltas</param>
        /// <returns>Lista de alertas del alumno</returns>
        Task<IEnumerable<AlertaProximidadDto>> GetHistorialAlertasPorAlumnoAsync(int idAlumno, bool incluirResueltas = false);

        /// <summary>
        /// Obtiene alertas activas por padre
        /// </summary>
        /// <param name="idPadre">ID del padre</param>
        /// <returns>Lista de alertas activas para el padre</returns>
        Task<IEnumerable<AlertaProximidadDto>> GetAlertasActivasPorPadreAsync(string idPadre);

        /// <summary>
        /// Marca una alerta como resuelta
        /// </summary>
        /// <param name="idAlerta">ID de la alerta</param>
        /// <returns>True si se resolvió correctamente</returns>
        Task<bool> MarcarAlertaComoResueltaAsync(int idAlerta);

        /// <summary>
        /// Confirma que el padre recibió la alerta
        /// </summary>
        /// <param name="idAlerta">ID de la alerta</param>
        /// <returns>True si se confirmó correctamente</returns>
        Task<bool> ConfirmarRecepcionPadreAsync(int idAlerta);

        /// <summary>
        /// Obtiene una alerta específica por ID
        /// </summary>
        /// <param name="idAlerta">ID de la alerta</param>
        /// <returns>DTO de la alerta o null si no existe</returns>
        Task<AlertaProximidadDto?> GetAlertaPorIdAsync(int idAlerta);

        /// <summary>
        /// Obtiene alertas activas por tipo
        /// </summary>
        /// <param name="tipoAlerta">Tipo: "proximidad" o "retraso"</param>
        /// <returns>Lista de alertas del tipo especificado</returns>
        Task<IEnumerable<AlertaProximidadDto>> GetAlertasActivasPorTipoAsync(string tipoAlerta);

        /// <summary>
        /// Elimina alertas antiguas resueltas (limpieza automática)
        /// </summary>
        /// <param name="diasAntiguedad">Días de antigüedad para eliminar</param>
        /// <returns>Número de alertas eliminadas</returns>
        Task<int> LimpiarAlertasAntiguasAsync(int diasAntiguedad = 30);

        /// <summary>
        /// Actualiza el estado y datos de una alerta existente
        /// </summary>
        /// <param name="dto">Datos para actualizar</param>
        /// <returns>DTO de la alerta actualizada o null si no existe</returns>
        Task<AlertaProximidadDto?> ActualizarAlertaAsync(AlertaProximidadUpdateDto dto);

        /// <summary>
        /// ✨ MÓDULO 6: Genera datos de ejemplo para pruebas del sistema de alertas
        /// </summary>
        /// <returns>True si se generaron correctamente los datos de ejemplo</returns>
        Task<bool> GenerarDatosDeEjemploAsync();
    }
}