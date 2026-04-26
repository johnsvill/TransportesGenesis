using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportesGenesis.DTOs.Asistencia;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Controllers.Api
{
    [Route("api/asistencia")]
    [ApiController]
    public class AsistenciaController : ControllerBase
    {
        private readonly IAsistenciaService _asistenciaService;

        public AsistenciaController(IAsistenciaService asistenciaService)
        {
            _asistenciaService = asistenciaService;
        }

        /// <summary>
        /// Obtener resumen de asistencia de hoy para un alumno
        /// </summary>
        [HttpGet("alumno/{idAlumno}/hoy")]
        public async Task<ActionResult<AsistenciaResumenDto>> GetResumenHoy(int idAlumno)
        {
            try
            {
                var resumen = await _asistenciaService.GetResumenAsistenciaHoyAsync(idAlumno);

                if (resumen == null)
                    return NotFound(new { message = "Alumno no encontrado" });

                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener resumen", error = ex.Message });
            }
        }

        /// <summary>
        /// Confirmar asistencia para hoy
        /// </summary>
        [HttpPost("confirmar")]
        public async Task<ActionResult<AsistenciaDto>> ConfirmarAsistencia([FromBody] ConfirmarAsistenciaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = await _asistenciaService.ConfirmarAsistenciaParaHoyAsync(dto);

                if (resultado == null)
                    return BadRequest(new { message = "No se pudo confirmar la asistencia" });

                return Ok(new 
                { 
                    success = true, 
                    message = "Asistencia confirmada exitosamente",
                    data = resultado
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al confirmar asistencia", error = ex.Message });
            }
        }

        /// <summary>
        /// Verificar si se puede confirmar para la mañana
        /// </summary>
        [HttpGet("puede-confirmar/manana")]
        public async Task<ActionResult<bool>> PuedeConfirmarMañana()
        {
            var ahora = DateTime.Now;
            var puede = await _asistenciaService.PuedeConfirmarMañanaAsync(ahora);

            return Ok(new 
            { 
                puede, 
                horaActual = ahora.ToString("HH:mm"),
                horaLimite = "04:00",
                mensaje = puede 
                    ? "Aún puede confirmar para la mañana" 
                    : "Ya no es posible confirmar para la mañana (límite: 4:00 AM)"
            });
        }

        /// <summary>
        /// Verificar si se puede confirmar para la tarde
        /// </summary>
        [HttpGet("puede-confirmar/tarde")]
        public async Task<ActionResult<bool>> PuedeConfirmarTarde()
        {
            var ahora = DateTime.Now;
            var puede = await _asistenciaService.PuedeConfirmarTardeAsync(ahora);

            return Ok(new 
            { 
                puede, 
                horaActual = ahora.ToString("HH:mm"),
                horaLimite = "11:00",
                mensaje = puede 
                    ? "Aún puede confirmar para la tarde" 
                    : "Ya no es posible confirmar para la tarde (límite: 11:00 AM)"
            });
        }

        /// <summary>
        /// Obtener historial de asistencias
        /// </summary>
        [HttpGet("alumno/{idAlumno}/historial")]
        public async Task<ActionResult<IEnumerable<AsistenciaDto>>> GetHistorial(
            int idAlumno, 
            [FromQuery] DateTime? fechaInicio, 
            [FromQuery] DateTime? fechaFin)
        {
            try
            {
                var inicio = fechaInicio ?? DateTime.Today.AddMonths(-1);
                var fin = fechaFin ?? DateTime.Today;

                var historial = await _asistenciaService.GetHistorialAsistenciaAsync(idAlumno, inicio, fin);
                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener historial", error = ex.Message });
            }
        }
    }
}
