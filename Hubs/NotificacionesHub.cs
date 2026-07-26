using Microsoft.AspNetCore.SignalR;

namespace TransportesGenesis.Hubs
{
    /// <summary>
    /// Hub de SignalR para notificaciones en tiempo real
    /// </summary>
    public class NotificacionesHub : Hub
    {
        private readonly ILogger<NotificacionesHub> _logger;

        public NotificacionesHub(ILogger<NotificacionesHub> logger)
        {
            _logger = logger;
        }

        #region Conexión y Grupos

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            _logger.LogInformation($"[SIGNALR] Cliente conectado: {connectionId}");

            // TODO: Obtener rol del usuario desde Claims
            // var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            _logger.LogInformation($"[SIGNALR] Cliente desconectado: {connectionId}");

            if (exception != null)
            {
                _logger.LogError(exception, $"[SIGNALR] Error en desconexión: {connectionId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Agregar usuario a un grupo específico (por rol o IdBus)
        /// </summary>
        public async Task UnirseAGrupo(string nombreGrupo)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, nombreGrupo);
            _logger.LogInformation($"[SIGNALR] Cliente {Context.ConnectionId} unido a grupo: {nombreGrupo}");
        }

        /// <summary>
        /// Remover usuario de un grupo
        /// </summary>
        public async Task SalirDeGrupo(string nombreGrupo)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, nombreGrupo);
            _logger.LogInformation($"[SIGNALR] Cliente {Context.ConnectionId} salió de grupo: {nombreGrupo}");
        }

        #endregion

        #region Métodos Cliente → Servidor (Invocados desde JavaScript)

        /// <summary>
        /// Piloto reporta ubicación actual del bus
        /// </summary>
        public async Task ReportarUbicacionBus(int idBus, decimal latitud, decimal longitud)
        {
            _logger.LogInformation($"[SIGNALR] Bus {idBus} reporta ubicación: Lat {latitud}, Lon {longitud}");

            var ubicacionData = new
            {
                IdBus = idBus,
                Latitud = latitud,
                Longitud = longitud,
                FechaHora = DateTime.Now
            };

            // Broadcast a todos los padres del bus
            await Clients.Group($"Bus_{idBus}").SendAsync("UbicacionBusActualizada", ubicacionData);

            // También enviar a los Administradores para tracking en tiempo real
            await Clients.Group("Administradores").SendAsync("UbicacionBusActualizada", ubicacionData);
        }

        /// <summary>
        /// Piloto marca parada como completada
        /// </summary>
        public async Task NotificarParadaCompletada(int idParada, int idAlumno, string nombreAlumno)
        {
            _logger.LogInformation($"[SIGNALR] Parada {idParada} completada - Alumno: {nombreAlumno}");

            // Notificar al padre específico del alumno
            await Clients.Group($"Alumno_{idAlumno}").SendAsync("ParadaCompletada", new
            {
                IdParada = idParada,
                IdAlumno = idAlumno,
                NombreAlumno = nombreAlumno,
                Mensaje = $"✅ {nombreAlumno} ha sido recogido/dejado correctamente",
                FechaHora = DateTime.Now
            });
        }

        /// <summary>
        /// Monitor/piloto publica ruta demo (orden aleatorio) para que el padre redibuje el mapa.
        /// </summary>
        public async Task NotificarRutaDemoActualizada(int idBus, string tipoRuta, string paradasJson, bool esAleatoria, string descripcion)
        {
            _logger.LogInformation($"[SIGNALR] Ruta demo bus {idBus} turno {tipoRuta} (aleatoria={esAleatoria})");

            await Clients.Group($"Bus_{idBus}").SendAsync("RutaDemoActualizada", new
            {
                IdBus = idBus,
                TipoRuta = tipoRuta,
                ParadasJson = paradasJson,
                EsAleatoria = esAleatoria,
                Descripcion = descripcion,
                FechaHora = DateTime.Now
            });
        }

        /// <summary>
        /// Piloto reporta retraso en la ruta
        /// </summary>
        public async Task ReportarRetraso(int idBus, int minutosRetraso, string motivo)
        {
            _logger.LogInformation($"[SIGNALR] Bus {idBus} reporta retraso de {minutosRetraso} min: {motivo}");

            // Notificar a todos los padres del bus
            await Clients.Group($"Bus_{idBus}").SendAsync("RetrasoReportado", new
            {
                IdBus = idBus,
                MinutosRetraso = minutosRetraso,
                Motivo = motivo,
                Mensaje = $"⚠️ El bus está retrasado {minutosRetraso} minutos. Motivo: {motivo}",
                FechaHora = DateTime.Now
            });
        }

        /// <summary>
        /// Admin envía mensaje broadcast a todos los usuarios
        /// </summary>
        public async Task EnviarMensajeBroadcast(string titulo, string mensaje, string tipo)
        {
            _logger.LogInformation($"[SIGNALR] Broadcast enviado - Tipo: {tipo}, Título: {titulo}");

            // Enviar a TODOS los clientes conectados
            await Clients.All.SendAsync("MensajeBroadcast", new
            {
                Titulo = titulo,
                Mensaje = mensaje,
                Tipo = tipo, // "info", "warning", "error", "success"
                FechaHora = DateTime.Now
            });
        }

        /// <summary>
        /// Admin envía mensaje a un bus específico
        /// </summary>
        public async Task EnviarMensajeABus(int idBus, string mensaje)
        {
            _logger.LogInformation($"[SIGNALR] Mensaje enviado a Bus {idBus}: {mensaje}");

            await Clients.Group($"Bus_{idBus}").SendAsync("MensajeRecibido", new
            {
                IdBus = idBus,
                Mensaje = mensaje,
                FechaHora = DateTime.Now
            });
        }

        #endregion

        #region Métodos para Alertas de Proximidad (Módulo 3)

        /// <summary>
        /// Sistema envía alerta de proximidad a padres específicos
        /// </summary>
        public async Task EnviarAlertaProximidad(int idAlerta, int idBus, int? idAlumno, string mensaje, string tipoAlerta, int? paradasRestantes)
        {
            _logger.LogInformation($"[SIGNALR-ALERTA] Enviando alerta {tipoAlerta} - ID: {idAlerta}, Bus: {idBus}");

            var alertaData = new
            {
                IdAlerta = idAlerta,
                IdBus = idBus,
                IdAlumno = idAlumno,
                TipoAlerta = tipoAlerta,
                Mensaje = mensaje,
                ParadasRestantes = paradasRestantes,
                FechaHora = DateTime.Now,
                RequiereConfirmacion = tipoAlerta == "proximidad"
            };

            // Enviar a grupo del bus (todos los padres de ese bus)
            await Clients.Group($"Bus_{idBus}").SendAsync("AlertaRecibida", alertaData);

            // Si hay alumno específico, también enviar a su grupo
            if (idAlumno.HasValue)
            {
                await Clients.Group($"Alumno_{idAlumno.Value}").SendAsync("AlertaPersonal", alertaData);
            }
        }

        /// <summary>
        /// Padre confirma que recibió la alerta (Cliente → Servidor)
        /// </summary>
        public async Task ConfirmarAlerta(int idAlerta, string idPadre)
        {
            _logger.LogInformation($"[SIGNALR-ALERTA] Padre {idPadre} confirma alerta {idAlerta}");

            // Aquí podrías llamar al AlertaService para actualizar la BD
            // Pero para no romper nada, solo enviaremos confirmación

            // Notificar a admins que la alerta fue confirmada
            await Clients.Group("Administradores").SendAsync("AlertaConfirmada", new
            {
                IdAlerta = idAlerta,
                IdPadre = idPadre,
                FechaConfirmacion = DateTime.Now,
                Mensaje = $"✅ Alerta #{idAlerta} confirmada por padre"
            });
        }

        /// <summary>
        /// Sistema notifica resolución de alerta
        /// </summary>
        public async Task NotificarAlertaResuelta(int idAlerta, int idBus, string motivo)
        {
            _logger.LogInformation($"[SIGNALR-ALERTA] Alerta {idAlerta} resuelta - Bus: {idBus}");

            var resolucionData = new
            {
                IdAlerta = idAlerta,
                IdBus = idBus,
                Motivo = motivo,
                FechaResolucion = DateTime.Now,
                Mensaje = $"✅ Alerta #{idAlerta} ha sido resuelta: {motivo}"
            };

            // Notificar a grupo del bus
            await Clients.Group($"Bus_{idBus}").SendAsync("AlertaResuelta", resolucionData);
        }

        #endregion

        #region Métodos de Pausa / Reanudación de Ruta

        /// <summary>
        /// Piloto o monitor pausa la ruta en la coordenada actual.
        /// motivo: "espera_nino" | "incidente" | "manual"
        /// </summary>
        public async Task PausarRuta(int idBus, string motivo, decimal latitud, decimal longitud)
        {
            _logger.LogInformation($"[SIGNALR-PAUSA] Bus {idBus} pausado. Motivo: {motivo} en ({latitud},{longitud})");

            var descripcionMotivo = motivo switch
            {
                "espera_nino" => "⏳ Esperando al niño en la parada",
                "incidente"   => "🔧 Incidente en el bus (mecánico/llanta)",
                _             => "⏸️ Pausa manual activada"
            };

            var data = new
            {
                IdBus      = idBus,
                Motivo     = motivo,
                Descripcion = descripcionMotivo,
                Latitud    = latitud,
                Longitud   = longitud,
                Mensaje    = $"🚌 Bus #{idBus} detenido. {descripcionMotivo}",
                FechaHora  = DateTime.Now
            };

            // Notificar a padres del bus
            await Clients.Group($"Bus_{idBus}").SendAsync("RutaPausada", data);

            // Registrar en historial de alertas (grupo Administradores)
            await Clients.Group("Administradores").SendAsync("AlertaHistorial", new
            {
                Tipo      = "Pausa",
                IdBus     = idBus,
                Mensaje   = $"Ruta Bus #{idBus} pausada — {descripcionMotivo}",
                FechaHora = DateTime.Now
            });
        }

        /// <summary>
        /// Piloto o monitor reanuda la ruta desde la coordenada donde se pausó.
        /// </summary>
        public async Task ReanudarRuta(int idBus, decimal latitud, decimal longitud, int minutosEtaActualizado)
        {
            _logger.LogInformation($"[SIGNALR-PAUSA] Bus {idBus} reanudado desde ({latitud},{longitud}). ETA +{minutosEtaActualizado} min");

            var data = new
            {
                IdBus                  = idBus,
                Latitud                = latitud,
                Longitud               = longitud,
                MinutosEtaActualizado  = minutosEtaActualizado,
                Mensaje                = $"✅ Bus #{idBus} ha reanudado la ruta. ETA actualizado +{minutosEtaActualizado} min.",
                FechaHora              = DateTime.Now
            };

            // Notificar a padres del bus
            await Clients.Group($"Bus_{idBus}").SendAsync("RutaReanudada", data);

            // Registrar en historial de alertas
            await Clients.Group("Administradores").SendAsync("AlertaHistorial", new
            {
                Tipo      = "Reanudacion",
                IdBus     = idBus,
                Mensaje   = $"Ruta Bus #{idBus} reanudada desde coordenada ({latitud:F5},{longitud:F5})",
                FechaHora = DateTime.Now
            });
        }

        #endregion

        #region Métodos de Prueba (Testing)

        /// <summary>
        /// Echo test - Cliente envía mensaje y servidor lo devuelve
        /// </summary>
        public async Task Echo(string mensaje)
        {
            _logger.LogInformation($"[SIGNALR] Echo recibido: {mensaje}");
            await Clients.Caller.SendAsync("EchoResponse", $"Echo: {mensaje}");
        }

        /// <summary>
        /// Ping test - Cliente hace ping y servidor responde pong
        /// </summary>
        public async Task Ping()
        {
            _logger.LogInformation($"[SIGNALR] Ping recibido de {Context.ConnectionId}");
            await Clients.Caller.SendAsync("Pong", DateTime.Now);
        }

        #endregion
    }
}
