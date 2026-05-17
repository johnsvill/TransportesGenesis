using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.DTOs.Paradas;

namespace TransportesGenesis.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class ParadasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ParadasController> _logger;

        public ParadasController(ApplicationDbContext context, ILogger<ParadasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/paradas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParadaDto>>> GetParadas()
        {
            try
            {
                var paradas = await _context.ParadasDb
                    .Include(p => p.Ruta)
                    .Include(p => p.Alumno)
                    .OrderBy(p => p.Orden)
                    .Select(p => new ParadaDto
                    {
                        IdParada = p.IdParada,
                        IdRuta = p.IdRuta,
                        IdAlumno = p.IdAlumno,
                        Latitud = p.Latitud,
                        Longitud = p.Longitud,
                        Direccion = p.Direccion,
                        Orden = p.Orden,
                        HoraEstimada = p.HoraEstimada,
                        Completada = p.Completada,
                        Activo = p.Activo,
                        FechaRegistro = p.FechaRegistro,
                        NombreRuta = p.Ruta != null ? p.Ruta.Nombre : null,
                        NombreAlumno = p.Alumno != null ? p.Alumno.Nombre + " " + p.Alumno.Apellido : null
                    })
                    .ToListAsync();

                return Ok(paradas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener paradas");
                return StatusCode(500, "Error al obtener paradas");
            }
        }

        // GET: api/paradas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Parada>> GetParada(int id)
        {
            try
            {
                var parada = await _context.ParadasDb.FindAsync(id);

                if (parada == null)
                {
                    return NotFound($"Parada con ID {id} no encontrada");
                }

                return Ok(parada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener parada {Id}", id);
                return StatusCode(500, "Error al obtener parada");
            }
        }

        // POST: api/paradas
        [HttpPost]
        public async Task<ActionResult<Parada>> CreateParada([FromBody] Parada parada)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Establecer valores por defecto
                parada.Activo = parada.Activo;
                parada.FechaRegistro = DateTime.Now;

                _context.ParadasDb.Add(parada);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Parada con ID {Id} creada correctamente", parada.IdParada);

                return CreatedAtAction(nameof(GetParada), new { id = parada.IdParada }, parada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear parada");
                return StatusCode(500, "Error al crear parada: " + ex.Message);
            }
        }

        // PUT: api/paradas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParada(int id, [FromBody] Parada parada)
        {
            try
            {
                if (id != parada.IdParada)
                {
                    return BadRequest("El ID de la parada no coincide");
                }

                var paradaExistente = await _context.ParadasDb.FindAsync(id);
                if (paradaExistente == null)
                {
                    return NotFound($"Parada con ID {id} no encontrada");
                }

                // Actualizar solo los campos permitidos
                paradaExistente.Latitud = parada.Latitud;
                paradaExistente.Longitud = parada.Longitud;
                paradaExistente.Orden = parada.Orden;
                paradaExistente.Activo = parada.Activo;
                paradaExistente.Direccion = parada.Direccion;
                paradaExistente.HoraEstimada = parada.HoraEstimada;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Parada {Id} actualizada correctamente", id);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar parada {Id}", id);
                return StatusCode(409, "Error de concurrencia al actualizar parada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar parada {Id}", id);
                return StatusCode(500, "Error al actualizar parada: " + ex.Message);
            }
        }

        // DELETE: api/paradas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParada(int id)
        {
            try
            {
                var parada = await _context.ParadasDb.FindAsync(id);
                if (parada == null)
                {
                    return NotFound($"Parada con ID {id} no encontrada");
                }

                // Marcar como inactivo en lugar de eliminar (soft delete)
                parada.Activo = 0;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Parada {Id} marcada como inactiva", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar parada {Id}", id);
                return StatusCode(500, "Error al eliminar parada: " + ex.Message);
            }
        }
    }
}
