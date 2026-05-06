using Microsoft.AspNetCore.Mvc;
using TransportesGenesis.DTOs.Notificaciones;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Controllers
{
    /// <summary>
    /// Controlador de prueba para demostrar el sistema de alertas con SignalR
    /// Módulo 3: Integración con Hub de Notificaciones
    /// </summary>
    public class AlertasTestController : Controller
    {
        private readonly IAlertaService _alertaService;
        private readonly INotificacionService _notificacionService;

        public AlertasTestController(
            IAlertaService alertaService, 
            INotificacionService notificacionService)
        {
            _alertaService = alertaService;
            _notificacionService = notificacionService;
        }

        /// <summary>
        /// Página de prueba para alertas
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Simula una alerta de proximidad
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SimularAlertaProximidad()
        {
            try
            {
                var alertaDto = new AlertaProximidadCreateDto
                {
                    TipoAlerta = "proximidad",
                    Mensaje = "🚌 El bus está a 2 paradas de tu ubicación. Prepárate para abordar.",
                    IdBus = 1,
                    IdAlumno = 1,
                    Estado = "activo"
                };

                // El AlertaService se encarga de enviar la notificación SignalR automáticamente
                var alerta = await _alertaService.RegistrarAlertaAsync(alertaDto);

                return Json(new { 
                    success = true, 
                    message = "Alerta de proximidad enviada", 
                    alertaId = alerta.Id 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Simula una alerta de retraso
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SimularAlertaRetraso()
        {
            try
            {
                var alertaDto = new AlertaProximidadCreateDto
                {
                    TipoAlerta = "retraso",
                    Mensaje = "⚠️ El bus tiene un retraso de 15 minutos debido a tráfico intenso.",
                    IdBus = 1,
                    Estado = "activo"
                };

                var alerta = await _alertaService.RegistrarAlertaAsync(alertaDto);

                return Json(new { 
                    success = true, 
                    message = "Alerta de retraso enviada", 
                    alertaId = alerta.Id 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Resuelve una alerta
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ResolverAlerta(int idAlerta)
        {
            try
            {
                // El AlertaService se encarga de enviar la notificación de resolución automáticamente
                var resultado = await _alertaService.MarcarAlertaComoResueltaAsync(idAlerta);

                if (resultado)
                {
                    return Json(new { 
                        success = true, 
                        message = "Alerta resuelta correctamente" 
                    });
                }
                else
                {
                    return Json(new { 
                        success = false, 
                        message = "Alerta no encontrada" 
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Confirma una alerta
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ConfirmarAlerta(int idAlerta)
        {
            try
            {
                // El AlertaService se encarga de enviar la notificación de confirmación automáticamente
                var resultado = await _alertaService.ConfirmarRecepcionPadreAsync(idAlerta);

                if (resultado)
                {
                    return Json(new { 
                        success = true, 
                        message = "Alerta confirmada correctamente" 
                    });
                }
                else
                {
                    return Json(new { 
                        success = false, 
                        message = "Alerta no encontrada" 
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Obtiene alertas activas para mostrar en la interfaz
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAlertasActivas()
        {
            try
            {
                var alertasProximidad = await _alertaService.GetAlertasActivasPorTipoAsync("proximidad");
                var alertasRetraso = await _alertaService.GetAlertasActivasPorTipoAsync("retraso");

                return Json(new {
                    proximidad = alertasProximidad,
                    retraso = alertasRetraso
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Prueba de broadcast directo (sin registrar en BD)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PruebaBroadcast()
        {
            try
            {
                await _notificacionService.EnviarBroadcastAsync(
                    "Prueba de Sistema", 
                    "Este es un mensaje de prueba del sistema de alertas", 
                    "info");

                return Json(new { 
                    success = true, 
                    message = "Broadcast enviado" 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Envía notificación directa a un bus específico
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> NotificarBus(int idBus, string mensaje)
        {
            try
            {
                await _notificacionService.EnviarMensajeABusAsync(idBus, mensaje);

                return Json(new { 
                    success = true, 
                    message = $"Mensaje enviado al bus {idBus}" 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
        }
    }
}