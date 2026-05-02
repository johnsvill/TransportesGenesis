using Microsoft.AspNetCore.Mvc;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Controllers.Api
{
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

        [HttpPost]
        public async Task<ActionResult<UbicacionBusDto>> RegistrarUbicacion([FromBody] UbicacionBusCreateDto dto)
        {
            try
            {
                var ubicacion = await _ubicacionService.RegistrarUbicacionAsync(dto);
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
