using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Repositories.Interfaces
{
    /// <summary>
    /// Repositorio para gestionar operaciones de datos de AlertaProximidad
    /// </summary>
    public interface IAlertaProximidadRepository : IRepositoryBase<AlertaProximidad>
    {
        /// <summary>
        /// Obtiene alertas por ID de bus
        /// </summary>
        /// <param name="idBus">ID del bus</param>
        /// <param name="incluirResueltas">Incluir alertas resueltas</param>
        /// <returns>Lista de alertas del bus</returns>
        Task<IEnumerable<AlertaProximidad>> GetAlertasPorBusAsync(int idBus, bool incluirResueltas = false);

        /// <summary>
        /// Obtiene alertas por ID de alumno
        /// </summary>
        /// <param name="idAlumno">ID del alumno</param>
        /// <param name="incluirResueltas">Incluir alertas resueltas</param>
        /// <returns>Lista de alertas del alumno</returns>
        Task<IEnumerable<AlertaProximidad>> GetAlertasPorAlumnoAsync(int idAlumno, bool incluirResueltas = false);

        /// <summary>
        /// Obtiene alertas activas por ID de padre
        /// </summary>
        /// <param name="idPadre">ID del padre</param>
        /// <returns>Lista de alertas activas para el padre</returns>
        Task<IEnumerable<AlertaProximidad>> GetAlertasActivasPorPadreAsync(string idPadre);

        /// <summary>
        /// Obtiene alertas por tipo y estado
        /// </summary>
        /// <param name="tipoAlerta">Tipo de alerta</param>
        /// <param name="estado">Estado de la alerta</param>
        /// <returns>Lista de alertas</returns>
        Task<IEnumerable<AlertaProximidad>> GetAlertasPorTipoYEstadoAsync(string tipoAlerta, string estado);

        /// <summary>
        /// Obtiene alertas más antiguas que la fecha especificada con estado resuelto
        /// </summary>
        /// <param name="fechaLimite">Fecha límite</param>
        /// <returns>Lista de alertas antiguas</returns>
        Task<IEnumerable<AlertaProximidad>> GetAlertasAntiguasResueltasAsync(DateTime fechaLimite);

        /// <summary>
        /// Marca una alerta como confirmada por el padre
        /// </summary>
        /// <param name="idAlerta">ID de la alerta</param>
        /// <returns>True si se actualizó correctamente</returns>
        Task<bool> ConfirmarRecepcionPadreAsync(int idAlerta);

        /// <summary>
        /// Marca una alerta como resuelta
        /// </summary>
        /// <param name="idAlerta">ID de la alerta</param>
        /// <returns>True si se actualizó correctamente</returns>
        Task<bool> MarcarComoResueltaAsync(int idAlerta);

        /// <summary>
        /// Elimina múltiples alertas por IDs
        /// </summary>
        /// <param name="ids">Lista de IDs a eliminar</param>
        /// <returns>Número de alertas eliminadas</returns>
        Task<int> DeleteMultipleAsync(IEnumerable<int> ids);
    }
}