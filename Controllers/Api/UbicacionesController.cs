using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionesController : ControllerBase
    {
        private readonly IUbicacionBusService _ubicacionService;

        public UbicacionesController(IUbicacionBusService ubicacionService)
        {
            _ubicacionService = ubicacionService;
        }

        [HttpGet("buses-activos")]
        public async Task<ActionResult<IEnumerable<UbicacionBusEnMapaDto>>> GetBusesActivos()
        {
            try
            {
                var ubicaciones = await _ubicacionService.GetUbicacionesBusesActivosAsync();
                return Ok(ubicaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{idBus}/ultima")]
        public async Task<ActionResult<UbicacionBusDto>> GetUltimaUbicacion(int idBus)
        {
            try
            {
                var ubicacion = await _ubicacionService.GetUltimaUbicacionAsync(idBus);
                if (ubicacion == null)
                    return NotFound(new { error = $"No se encontró ubicación para el bus {idBus}" });

                return Ok(ubicacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Monitor,Piloto")]
        [HttpPost]
        public async Task<ActionResult<UbicacionBusDto>> RegistrarUbicacion([FromBody] UbicacionBusCreateDto dto)
        {
            try
            {
                // Usar el método que registra y además notifica via SignalR (proximidad / llegada)
                var ubicacion = await _ubicacionService.RegistrarUbicacionYNotificarAsync(dto);
                return CreatedAtAction(nameof(GetUltimaUbicacion), new { idBus = ubicacion.IdBus }, ubicacion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Sincroniza un lote de puntos encolados offline (orden cronológico en el cliente).
        /// Persiste cada punto y notifica SignalR; el último actualiza la UI en vivo.
        /// </summary>
        [Authorize(Roles = "Monitor,Piloto")]
        [HttpPost("lote")]
        public async Task<ActionResult<object>> RegistrarUbicacionesLote([FromBody] List<UbicacionBusCreateDto> puntos)
        {
            if (puntos == null || puntos.Count == 0)
                return BadRequest(new { error = "El lote está vacío." });

            if (puntos.Count > 500)
                return BadRequest(new { error = "El lote no puede exceder 500 puntos." });

            try
            {
                var guardados = 0;
                UbicacionBusDto? ultima = null;

                foreach (var dto in puntos)
                {
                    ultima = await _ubicacionService.RegistrarUbicacionYNotificarAsync(dto);
                    guardados++;
                }

                return Ok(new
                {
                    guardados,
                    ultima
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{idBus}/historial")]
        public async Task<ActionResult<IEnumerable<UbicacionBusDto>>> GetHistorial(
            int idBus,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                var historial = await _ubicacionService.GetHistorialAsync(idBus, fechaInicio, fechaFin);
                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
