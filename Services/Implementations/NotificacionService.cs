using Microsoft.AspNetCore.SignalR;
using TransportesGenesis.Hubs;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de notificaciones usando SignalR
    /// </summary>
    public class NotificacionService : INotificacionService
    {
        private readonly IHubContext<NotificacionesHub> _hubContext;
        private readonly ILogger<NotificacionService> _logger;

        public NotificacionService(
            IHubContext<NotificacionesHub> hubContext,
            ILogger<NotificacionService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotificarBusCercaAsync(int idBus, int idParada, decimal distanciaKm)
        {
            try
            {
                _logger.LogInformation($"[NOTIFICACION] Bus {idBus} cerca de parada {idParada} - Distancia: {distanciaKm:F2} km");

                var mensaje = new
                {
                    IdBus = idBus,
                    IdParada = idParada,
                    DistanciaKm = distanciaKm,
                    Mensaje = $"🚌 El bus está a {distanciaKm:F2} km de la parada. Prepárate para abordar.",
                    FechaHora = DateTime.Now
                };

                // Enviar a grupo del bus
                await _hubContext.Clients.Group($"Bus_{idBus}")
                    .SendAsync("BusCerca", mensaje);

                _logger.LogInformation($"[NOTIFICACION] Notificación 'BusCerca' enviada al grupo Bus_{idBus}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[NOTIFICACION] Error al notificar bus cerca: Bus {idBus}, Parada {idParada}");
                throw;
            }
        }

        public async Task NotificarParadaCompletadaAsync(int idAlumno, string nombreAlumno, int idParada)
        {
            try
            {
                _logger.LogInformation($"[NOTIFICACION] Parada {idParada} completada - Alumno {idAlumno}: {nombreAlumno}");

                var mensaje = new
                {
                    IdAlumno = idAlumno,
                    IdParada = idParada,
                    NombreAlumno = nombreAlumno,
                    Mensaje = $"✅ {nombreAlumno} ha sido recogido/dejado en la parada.",
                    FechaHora = DateTime.Now
                };

                // Enviar a grupo específico del alumno (sus padres)
                await _hubContext.Clients.Group($"Alumno_{idAlumno}")
                    .SendAsync("ParadaCompletada", mensaje);

                _logger.LogInformation($"[NOTIFICACION] Notificación 'ParadaCompletada' enviada al grupo Alumno_{idAlumno}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[NOTIFICACION] Error al notificar parada completada: Alumno {idAlumno}, Parada {idParada}");
                throw;
            }
        }

        public async Task NotificarRetrasoAsync(int idBus, int minutosRetraso, string motivo)
        {
            try
            {
                _logger.LogInformation($"[NOTIFICACION] Retraso reportado - Bus {idBus}: {minutosRetraso} min - {motivo}");

                var mensaje = new
                {
                    IdBus = idBus,
                    MinutosRetraso = minutosRetraso,
                    Motivo = motivo,
                    Mensaje = $"⚠️ El bus tiene un retraso de {minutosRetraso} minutos. Motivo: {motivo}",
                    FechaHora = DateTime.Now
                };

                // Enviar a todos los padres del bus
                await _hubContext.Clients.Group($"Bus_{idBus}")
                    .SendAsync("RetrasoReportado", mensaje);

                _logger.LogInformation($"[NOTIFICACION] Notificación 'RetrasoReportado' enviada al grupo Bus_{idBus}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[NOTIFICACION] Error al notificar retraso: Bus {idBus}");
                throw;
            }
        }

        public async Task EnviarBroadcastAsync(string titulo, string mensaje, string tipo)
        {
            try
            {
                _logger.LogInformation($"[NOTIFICACION] Broadcast enviado - Tipo: {tipo}, Título: {titulo}");

                var payload = new
                {
                    Titulo = titulo,
                    Mensaje = mensaje,
                    Tipo = tipo, // "info", "warning", "error", "success"
                    FechaHora = DateTime.Now
                };

                // Enviar a TODOS los clientes conectados
                await _hubContext.Clients.All
                    .SendAsync("MensajeBroadcast", payload);

                _logger.LogInformation($"[NOTIFICACION] Broadcast enviado a todos los clientes");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[NOTIFICACION] Error al enviar broadcast");
                throw;
            }
        }

        public async Task EnviarMensajeABusAsync(int idBus, string mensaje)
        {
            try
            {
                _logger.LogInformation($"[NOTIFICACION] Mensaje enviado a Bus {idBus}: {mensaje}");

                var payload = new
                {
                    IdBus = idBus,
                    Mensaje = mensaje,
                    FechaHora = DateTime.Now
                };

                // Enviar a grupo del bus
                await _hubContext.Clients.Group($"Bus_{idBus}")
                    .SendAsync("MensajeRecibido", payload);

                _logger.LogInformation($"[NOTIFICACION] Mensaje enviado al grupo Bus_{idBus}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[NOTIFICACION] Error al enviar mensaje a bus {idBus}");
                throw;
            }
        }

        public async Task ActualizarUbicacionBusAsync(int idBus, decimal latitud, decimal longitud)
        {
            try
            {
                // Log solo cada 10 actualizaciones para no saturar logs
                // (se puede mejorar con contador en memoria)
                _logger.LogDebug($"[NOTIFICACION] Ubicación Bus {idBus} actualizada: Lat {latitud}, Lon {longitud}");

                var payload = new
                {
                    IdBus = idBus,
                    Latitud = latitud,
                    Longitud = longitud,
                    FechaHora = DateTime.Now
                };

                // Enviar a grupo del bus
                await _hubContext.Clients.Group($"Bus_{idBus}")
                    .SendAsync("UbicacionBusActualizada", payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[NOTIFICACION] Error al actualizar ubicación Bus {idBus}");
                // No throw - actualización de ubicación no debe romper flujo
            }
        }
    }
}
