using TransportesGenesis.DTOs.Notificaciones;

namespace TransportesGenesis.Services.Interfaces
{
    /// <summary>
    /// Servicio para enviar notificaciones en tiempo real via SignalR
    /// </summary>
    public interface INotificacionService
    {
        /// <summary>
        /// Notificar a padres cuando bus se acerca a parada (< 1 km)
        /// </summary>
        Task NotificarBusCercaAsync(int idBus, int idParada, decimal distanciaKm);

        /// <summary>
        /// Notificar a padre cuando parada es completada
        /// </summary>
        Task NotificarParadaCompletadaAsync(int idAlumno, string nombreAlumno, int idParada);

        /// <summary>
        /// Notificar retraso en ruta a todos los padres del bus
        /// </summary>
        Task NotificarRetrasoAsync(int idBus, int minutosRetraso, string motivo);

        /// <summary>
        /// Broadcast mensaje a todos los usuarios conectados
        /// </summary>
        Task EnviarBroadcastAsync(string titulo, string mensaje, string tipo);

        /// <summary>
        /// Enviar mensaje a todos los usuarios de un bus específico
        /// </summary>
        Task EnviarMensajeABusAsync(int idBus, string mensaje);

        /// <summary>
        /// Actualizar ubicación del bus en tiempo real
        /// </summary>
        Task ActualizarUbicacionBusAsync(int idBus, decimal latitud, decimal longitud);

        /// <summary>
        /// Enviar alerta de proximidad en tiempo real via SignalR
        /// </summary>
        Task EnviarAlertaProximidadAsync(int idAlerta, int idBus, int? idAlumno, string mensaje, string tipoAlerta, int? paradasRestantes = null);

        /// <summary>
        /// Notificar que una alerta ha sido resuelta
        /// </summary>
        Task NotificarAlertaResueltaAsync(int idAlerta, int idBus, string motivo);

        /// <summary>
        /// Enviar confirmación de alerta recibida por padre
        /// </summary>
        Task NotificarConfirmacionAlertaAsync(int idAlerta, string idPadre);
    }
}
