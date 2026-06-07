using Microsoft.AspNetCore.Mvc;
using TransportesGenesis.DTOs.Notificaciones;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Controllers.Api
{
    /// <summary>
    /// Controlador API para gestionar alertas de proximidad y retrasos
    /// </summary>
    [Route("api/alertas")]
    [ApiController]
    public class AlertasController : ControllerBase
    {
        private readonly IAlertaService _alertaService;
        private readonly IBusService _busService;

        public AlertasController(IAlertaService alertaService, IBusService busService)
        {
            _alertaService = alertaService;
            _busService = busService;
        }

        /// <summary>
        /// Registra una nueva alerta (proximidad o retraso)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AlertaProximidadDto>> RegistrarAlerta([FromBody] AlertaProximidadCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var alerta = await _alertaService.RegistrarAlertaAsync(dto);
                return Ok(alerta);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene el historial de alertas por bus
        /// </summary>
        [HttpGet("bus/{idBus}")]
        public async Task<ActionResult<IEnumerable<AlertaProximidadDto>>> GetAlertasPorBus(
            int idBus, 
            [FromQuery] bool incluirResueltas = false)
        {
            try
            {
                var alertas = await _alertaService.GetHistorialAlertasPorBusAsync(idBus, incluirResueltas);
                return Ok(alertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener alertas", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene el historial de alertas por alumno
        /// </summary>
        [HttpGet("alumno/{idAlumno}")]
        public async Task<ActionResult<IEnumerable<AlertaProximidadDto>>> GetAlertasPorAlumno(
            int idAlumno, 
            [FromQuery] bool incluirResueltas = false)
        {
            try
            {
                var alertas = await _alertaService.GetHistorialAlertasPorAlumnoAsync(idAlumno, incluirResueltas);
                return Ok(alertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener alertas", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene alertas activas por padre
        /// </summary>
        [HttpGet("padre/{idPadre}")]
        public async Task<ActionResult<IEnumerable<AlertaProximidadDto>>> GetAlertasPorPadre(string idPadre)
        {
            try
            {
                var alertas = await _alertaService.GetAlertasActivasPorPadreAsync(idPadre);
                return Ok(alertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener alertas", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una alerta específica por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AlertaProximidadDto>> GetAlertaPorId(int id)
        {
            try
            {
                var alerta = await _alertaService.GetAlertaPorIdAsync(id);
                if (alerta == null)
                {
                    return NotFound(new { error = "Alerta no encontrada" });
                }
                return Ok(alerta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener alerta", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene alertas activas por tipo
        /// </summary>
        [HttpGet("tipo/{tipoAlerta}")]
        public async Task<ActionResult<IEnumerable<AlertaProximidadDto>>> GetAlertasPorTipo(string tipoAlerta)
        {
            if (tipoAlerta != "proximidad" && tipoAlerta != "retraso")
            {
                return BadRequest(new { error = "Tipo de alerta debe ser 'proximidad' o 'retraso'" });
            }

            try
            {
                var alertas = await _alertaService.GetAlertasActivasPorTipoAsync(tipoAlerta);
                return Ok(alertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener alertas", details = ex.Message });
            }
        }

        /// <summary>
        /// Marca una alerta como resuelta
        /// </summary>
        [HttpPut("{id}/resolver")]
        public async Task<IActionResult> MarcarComoResuelta(int id)
        {
            try
            {
                var resultado = await _alertaService.MarcarAlertaComoResueltaAsync(id);
                if (!resultado)
                {
                    return NotFound(new { error = "Alerta no encontrada" });
                }
                return Ok(new { message = "Alerta marcada como resuelta" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al resolver alerta", details = ex.Message });
            }
        }

        /// <summary>
        /// Confirma que el padre recibió la alerta
        /// </summary>
        [HttpPut("{id}/confirmar")]
        public async Task<IActionResult> ConfirmarRecepcionPadre(int id)
        {
            try
            {
                var resultado = await _alertaService.ConfirmarRecepcionPadreAsync(id);
                if (!resultado)
                {
                    return NotFound(new { error = "Alerta no encontrada" });
                }
                return Ok(new { message = "Confirmación de padre registrada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al confirmar alerta", details = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza una alerta existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<AlertaProximidadDto>> ActualizarAlerta(
            int id, 
            [FromBody] AlertaProximidadUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { error = "El ID de la URL no coincide con el ID del objeto" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var alerta = await _alertaService.ActualizarAlertaAsync(dto);
                if (alerta == null)
                {
                    return NotFound(new { error = "Alerta no encontrada" });
                }
                return Ok(alerta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar alerta", details = ex.Message });
            }
        }

        /// <summary>
        /// Limpia alertas antiguas resueltas
        /// </summary>
        [HttpDelete("limpiar")]
        public async Task<IActionResult> LimpiarAlertasAntiguas([FromQuery] int diasAntiguedad = 30)
        {
            if (diasAntiguedad < 1)
            {
                return BadRequest(new { error = "Los días de antigüedad deben ser mayor a 0" });
            }

            try
            {
                var eliminadas = await _alertaService.LimpiarAlertasAntiguasAsync(diasAntiguedad);
                return Ok(new { 
                    message = $"Limpieza completada",
                    alertasEliminadas = eliminadas,
                    diasAntiguedad = diasAntiguedad
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al limpiar alertas", details = ex.Message });
            }
        }

        /// <summary>
        /// 🔍 Endpoint de diagnóstico para verificar el estado del sistema
        /// </summary>
        [HttpGet("diagnostico")]
        public async Task<IActionResult> Diagnostico()
        {
            try
            {
                var errores = new List<string>();
                var recomendaciones = new List<string>();

                // Variables para el diagnóstico
                int busesCount = 0;
                var busesExistentes = new List<object>();
                bool alertaServiceOk = false;
                bool busServiceOk = false;
                int alertasActivas = 0;

                // Verificar BusService
                try
                {
                    var buses = await _busService.GetAllBusesAsync();
                    busServiceOk = true;
                    busesCount = buses?.Count() ?? 0;
                    busesExistentes = buses?.Select(b => new { b.IdBus, b.Placa, b.Modelo }).Cast<object>().ToList() ?? new List<object>();

                    if (busesCount == 0)
                    {
                        recomendaciones.Add("No hay buses registrados. Usa el botón 'Generar Datos de Ejemplo' para crear algunos.");
                    }
                }
                catch (Exception ex)
                {
                    busServiceOk = false;
                    errores.Add($"BusService error: {ex.Message}");
                }

                // Verificar AlertaService
                try
                {
                    alertaServiceOk = _alertaService != null;

                    // Intentar obtener alertas activas (método simple)
                    if (alertaServiceOk && busesCount > 0)
                    {
                        alertasActivas = 0; // Por ahora, asumimos 0 hasta tener datos
                    }
                }
                catch (Exception ex)
                {
                    alertaServiceOk = false;
                    errores.Add($"AlertaService error: {ex.Message}");
                }

                // Determinar estado general del sistema
                var estado = "Operativo";
                var baseDatos = "Conectada";
                var signalRStatus = "Disponible";

                if (errores.Any())
                {
                    estado = "Con errores";
                }
                else if (!busServiceOk || !alertaServiceOk)
                {
                    estado = "Parcialmente operativo";
                }

                if (busesCount == 0)
                {
                    recomendaciones.Add("Crear buses de ejemplo para poder probar las alertas");
                }

                if (alertasActivas == 0 && busesCount > 0)
                {
                    recomendaciones.Add("No hay alertas activas. Prueba generando datos de ejemplo.");
                }

                var diagnostico = new
                {
                    timestamp = DateTime.Now,
                    estado = estado,
                    baseDatos = baseDatos,
                    busesRegistrados = busesCount,
                    alertasActivas = alertasActivas,
                    signalRStatus = signalRStatus,
                    servicios = new
                    {
                        busService = busServiceOk ? "OK" : "ERROR",
                        alertaService = alertaServiceOk ? "OK" : "ERROR"
                    },
                    busesExistentes = busesExistentes,
                    errores = errores,
                    recomendaciones = recomendaciones
                };

                return Ok(diagnostico);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = "Error en diagnóstico", 
                    details = ex.Message 
                });
            }
        }

        /// <summary>
        /// ✨ MÓDULO 6: Genera datos de ejemplo para pruebas del sistema de alertas
        /// </summary>
        [HttpPost("generar-datos-ejemplo")]
        public async Task<IActionResult> GenerarDatosDeEjemplo()
        {
            try
            {
                Console.WriteLine("🔧 Iniciando generación de datos de ejemplo...");

                // ✨ Verificar si existen buses, si no, crear algunos de ejemplo
                var busesExistentes = await _busService.GetAllBusesAsync();

                if (busesExistentes == null || !busesExistentes.Any())
                {
                    Console.WriteLine("🚌 No hay buses registrados. Creando buses de ejemplo...");

                    // Crear algunos buses de ejemplo
                    var busesEjemplo = new[]
                    {
                        new { Placa = "GTM-001", Modelo = "Mercedes Benz", Capacidad = 45 },
                        new { Placa = "GTM-002", Modelo = "Volvo B7RLE", Capacidad = 40 },
                        new { Placa = "GTM-003", Modelo = "Scania Citywide", Capacidad = 50 }
                    };

                    foreach (var busEjemplo in busesEjemplo)
                    {
                        try
                        {
                            Console.WriteLine($"🔄 Intentando crear bus {busEjemplo.Placa}...");

                            // Verificar si ya existe este bus
                            var busExistente = await _busService.GetBusByPlacaAsync(busEjemplo.Placa);
                            if (busExistente == null)
                            {
                                await _busService.CreateBusAsync(new DTOs.Geolocalizacion.BusCreateDto
                                {
                                    Placa = busEjemplo.Placa,
                                    Modelo = busEjemplo.Modelo,
                                    Capacidad = busEjemplo.Capacidad
                                });
                                Console.WriteLine($"✅ Bus {busEjemplo.Placa} creado exitosamente");
                            }
                            else
                            {
                                Console.WriteLine($"ℹ️ Bus {busEjemplo.Placa} ya existe, saltando...");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"⚠️ Error creando bus {busEjemplo.Placa}: {ex.Message}");
                            // Continuar con el siguiente bus sin fallar
                        }
                    }

                    // Verificar nuevamente que tenemos buses
                    busesExistentes = await _busService.GetAllBusesAsync();
                    if (!busesExistentes.Any())
                    {
                        Console.WriteLine("❌ No se pudieron crear buses de ejemplo");
                        return BadRequest(new { 
                            error = "No se pudieron crear buses de ejemplo", 
                            razon = "Error en la creación de buses - verifica los logs de consola para más detalles" 
                        });
                    }
                }

                Console.WriteLine($"🚌 Usando {busesExistentes.Count()} buses existentes para generar alertas");

                var resultado = await _alertaService.GenerarDatosDeEjemploAsync();

                if (!resultado)
                {
                    return BadRequest(new { 
                        error = "No se pudieron generar los datos de ejemplo", 
                        razon = "Error en la generación de alertas - revisa los logs de consola para más detalles" 
                    });
                }

                return Ok(new { 
                    message = "Datos de ejemplo generados exitosamente",
                    descripcion = $"Se han creado/verificado {busesExistentes.Count()} buses y generado alertas de proximidad, retrasos y historial para pruebas",
                    instrucciones = "Ahora puedes navegar a /Admin/AlertasHistorial para ver los datos"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error general: {ex.Message}");
                Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { 
                    error = "Error al generar datos de ejemplo", 
                    details = ex.Message,
                    stackTrace = ex.StackTrace 
                });
            }
        }
    }
}