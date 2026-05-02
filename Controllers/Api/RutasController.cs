using Microsoft.AspNetCore.Mvc;
using TransportesGenesis.DTOs.Ruta;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RutasController : ControllerBase
    {
        private readonly IRutaService _rutaService;

        public RutasController(IRutaService rutaService)
        {
            _rutaService = rutaService;
        }

        /// <summary>
        /// POST /api/rutas/calcular
        /// Calcula la ruta óptima para un bus en una fecha y turno específico
        /// </summary>
        [HttpPost("calcular")]
        public async Task<ActionResult<object>> CalcularRutaOptimizada([FromBody] CalcularRutaDto dto)
        {
            try
            {
                Console.WriteLine($"[API RUTAS] POST /calcular - Bus: {dto.IdBus}, Fecha: {dto.Fecha:dd/MM/yyyy}, Tipo: {dto.TipoRuta}");

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("[API RUTAS] ❌ Modelo inválido");
                    return BadRequest(new
                    {
                        success = false,
                        message = "Datos inválidos",
                        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                var ruta = await _rutaService.CalcularRutaOptimizadaAsync(dto);

                if (ruta == null || !ruta.Paradas.Any())
                {
                    Console.WriteLine("[API RUTAS] ⚠️ No se encontraron alumnos para generar ruta");
                    return Ok(new
                    {
                        success = false,
                        message = "No hay alumnos confirmados para este bus en la fecha indicada",
                        data = (object)null
                    });
                }

                Console.WriteLine($"[API RUTAS] ✅ Ruta calculada exitosamente - ID: {ruta.IdRuta}, Paradas: {ruta.Paradas.Count}");

                return Ok(new
                {
                    success = true,
                    message = $"Ruta calculada exitosamente con {ruta.Paradas.Count} paradas",
                    data = ruta
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API RUTAS] ❌ Error: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al calcular la ruta",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// GET /api/rutas/{id}
        /// Obtiene una ruta específica con todas sus paradas
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRutaPorId(int id)
        {
            try
            {
                Console.WriteLine($"[API RUTAS] GET /{id}");

                var ruta = await _rutaService.GetRutaConParadasAsync(id);

                if (ruta == null)
                {
                    Console.WriteLine($"[API RUTAS] ⚠️ Ruta {id} no encontrada");
                    return NotFound(new
                    {
                        success = false,
                        message = "Ruta no encontrada"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = ruta
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API RUTAS] ❌ Error: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener la ruta",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// GET /api/rutas/bus/{idBus}/activa
        /// Obtiene la ruta activa de un bus para hoy
        /// </summary>
        [HttpGet("bus/{idBus}/activa")]
        public async Task<ActionResult<object>> GetRutaActivaDelBus(int idBus, [FromQuery] string? tipoRuta = null)
        {
            try
            {
                var fecha = DateTime.Now.Date;
                var tipo = tipoRuta ?? (DateTime.Now.Hour < 12 ? "Mañana" : "Tarde");

                Console.WriteLine($"[API RUTAS] GET /bus/{idBus}/activa - Fecha: {fecha:dd/MM/yyyy}, Tipo: {tipo}");

                var ruta = await _rutaService.GetRutaActivaDelBusAsync(idBus, fecha, tipo);

                if (ruta == null)
                {
                    Console.WriteLine($"[API RUTAS] ⚠️ No hay ruta activa para Bus {idBus}");

                    // Modo simulación: retornar ruta de ejemplo
                    return Ok(new
                    {
                        success = false,
                        message = "No hay ruta activa para este bus. Puede calcular una nueva ruta.",
                        data = new
                        {
                            idBus,
                            tipoRuta = tipo,
                            fecha,
                            mensaje = "Sin ruta generada"
                        }
                    });
                }

                Console.WriteLine($"[API RUTAS] ✅ Ruta encontrada - ID: {ruta.IdRuta}, Paradas: {ruta.Paradas.Count}");

                return Ok(new
                {
                    success = true,
                    data = ruta
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API RUTAS] ❌ Error: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener la ruta activa",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// PUT /api/rutas/{idRuta}/parada/{idParada}/completar
        /// Marca una parada como completada o no completada
        /// </summary>
        [HttpPut("{idRuta}/parada/{idParada}/completar")]
        public async Task<ActionResult<object>> MarcarParadaCompletada(int idRuta, int idParada, [FromBody] MarcarParadaDto dto)
        {
            try
            {
                Console.WriteLine($"[API RUTAS] PUT /{idRuta}/parada/{idParada}/completar - Completada: {dto.Completada}");

                if (dto.IdParada != idParada)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "El ID de parada no coincide"
                    });
                }

                var resultado = await _rutaService.MarcarParadaCompletadaAsync(dto);

                if (!resultado)
                {
                    Console.WriteLine($"[API RUTAS] ⚠️ No se pudo marcar la parada {idParada}");
                    return NotFound(new
                    {
                        success = false,
                        message = "Parada no encontrada"
                    });
                }

                Console.WriteLine($"[API RUTAS] ✅ Parada {idParada} marcada como completada: {dto.Completada}");

                return Ok(new
                {
                    success = true,
                    message = dto.Completada ? "Parada marcada como completada" : "Parada marcada como pendiente"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API RUTAS] ❌ Error: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al marcar la parada",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// GET /api/rutas/bus/{idBus}/fecha/{fecha}
        /// Obtiene las rutas de un bus para una fecha específica
        /// </summary>
        [HttpGet("bus/{idBus}/fecha/{fecha}")]
        public async Task<ActionResult<object>> GetRutasPorBusYFecha(int idBus, DateTime fecha)
        {
            try
            {
                Console.WriteLine($"[API RUTAS] GET /bus/{idBus}/fecha/{fecha:dd/MM/yyyy}");

                var rutas = await _rutaService.GetRutasByBusAsync(idBus);

                return Ok(new
                {
                    success = true,
                    data = rutas
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API RUTAS] ❌ Error: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener las rutas",
                    error = ex.Message
                });
            }
        }
    }
}
