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

        [HttpGet("{id}")]
        public async Task<ActionResult<ParadaDto>> GetParada(int id)
        {
            try
            {
                var parada = await ObtenerParadaDtoAsync(id);

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

        [HttpPost]
        public async Task<ActionResult<ParadaDto>> CreateParada([FromBody] Parada parada)
        {
            try
            {
                var errorValidacion = await ValidarParadaAsync(parada.IdRuta, parada.IdAlumno, null);
                if (errorValidacion != null)
                {
                    return BadRequest(new { message = errorValidacion });
                }

                if (parada.Orden <= 0)
                {
                    var maxOrden = await _context.ParadasDb
                        .Where(p => p.IdRuta == parada.IdRuta)
                        .Select(p => (int?)p.Orden)
                        .MaxAsync() ?? 0;
                    parada.Orden = maxOrden + 1;
                }

                var nuevaParada = new Parada
                {
                    IdRuta = parada.IdRuta,
                    IdAlumno = parada.IdAlumno,
                    Latitud = parada.Latitud,
                    Longitud = parada.Longitud,
                    Direccion = parada.Direccion,
                    Orden = parada.Orden,
                    HoraEstimada = parada.HoraEstimada,
                    Activo = parada.Activo,
                    FechaRegistro = DateTime.Now,
                    Completada = false
                };

                _context.ParadasDb.Add(nuevaParada);
                await _context.SaveChangesAsync();

                await AsignarBusAlAlumnoSiAplicaAsync(nuevaParada.IdRuta, nuevaParada.IdAlumno);

                _logger.LogInformation("Parada con ID {Id} creada correctamente", nuevaParada.IdParada);

                var dto = await ObtenerParadaDtoAsync(nuevaParada.IdParada);
                return CreatedAtAction(nameof(GetParada), new { id = nuevaParada.IdParada }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear parada");
                return StatusCode(500, new { message = "Error al crear parada: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParada(int id, [FromBody] Parada parada)
        {
            try
            {
                if (id != parada.IdParada)
                {
                    return BadRequest(new { message = "El ID de la parada no coincide" });
                }

                var paradaExistente = await _context.ParadasDb.FindAsync(id);
                if (paradaExistente == null)
                {
                    return NotFound(new { message = $"Parada con ID {id} no encontrada" });
                }

                var idRuta = parada.IdRuta > 0 ? parada.IdRuta : paradaExistente.IdRuta;
                var errorValidacion = await ValidarParadaAsync(idRuta, parada.IdAlumno, id);
                if (errorValidacion != null)
                {
                    return BadRequest(new { message = errorValidacion });
                }

                paradaExistente.Latitud = parada.Latitud;
                paradaExistente.Longitud = parada.Longitud;
                paradaExistente.Orden = parada.Orden;
                paradaExistente.Activo = parada.Activo;
                paradaExistente.Direccion = parada.Direccion;
                paradaExistente.HoraEstimada = parada.HoraEstimada;
                paradaExistente.IdAlumno = parada.IdAlumno;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Parada {Id} actualizada correctamente", id);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al actualizar parada {Id}", id);
                return StatusCode(409, new { message = "Error de concurrencia al actualizar parada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar parada {Id}", id);
                return StatusCode(500, new { message = "Error al actualizar parada: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParada(int id)
        {
            try
            {
                var parada = await _context.ParadasDb.FindAsync(id);
                if (parada == null)
                {
                    return NotFound(new { message = $"Parada con ID {id} no encontrada" });
                }

                parada.Activo = 0;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Parada {Id} marcada como inactiva", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar parada {Id}", id);
                return StatusCode(500, new { message = "Error al eliminar parada: " + ex.Message });
            }
        }

        private async Task<ParadaDto?> ObtenerParadaDtoAsync(int idParada)
        {
            return await _context.ParadasDb
                .Where(p => p.IdParada == idParada)
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
                .FirstOrDefaultAsync();
        }

        private async Task<string?> ValidarParadaAsync(int idRuta, int? idAlumno, int? idParadaExcluir)
        {
            if (idRuta <= 0)
                return "Seleccione una ruta válida.";

            var ruta = await _context.RutasDb.FirstOrDefaultAsync(r => r.IdRuta == idRuta);
            if (ruta == null)
                return "La ruta seleccionada no existe.";

            if (!ruta.EsActiva)
                return "La ruta seleccionada no está activa.";

            if (idAlumno.HasValue)
            {
                var alumno = await _context.AlumnosDb.FirstOrDefaultAsync(a => a.IdAlumno == idAlumno.Value);
                if (alumno == null)
                    return "El alumno seleccionado no existe.";

                if (alumno.IdBusAsignado.HasValue && alumno.IdBusAsignado != ruta.IdBus)
                    return "El alumno ya está asignado a otro bus.";

                var paradaEnOtraRuta = await _context.ParadasDb.AnyAsync(p =>
                    p.IdAlumno == idAlumno.Value &&
                    (!idParadaExcluir.HasValue || p.IdParada != idParadaExcluir.Value));

                if (paradaEnOtraRuta)
                    return "El alumno ya tiene una parada asignada en otra ruta.";

                var duplicada = await _context.ParadasDb.AnyAsync(p =>
                    p.IdRuta == idRuta &&
                    p.IdAlumno == idAlumno.Value &&
                    (!idParadaExcluir.HasValue || p.IdParada != idParadaExcluir.Value));

                if (duplicada)
                    return "Ya existe una parada para este alumno en la ruta seleccionada.";

                if (!alumno.IdBusAsignado.HasValue)
                {
                    var asignados = await _context.AlumnosDb.CountAsync(a => a.IdBusAsignado == ruta.IdBus);
                    var bus = await _context.BusesDb.FindAsync(ruta.IdBus);
                    if (bus != null && asignados >= bus.Capacidad)
                        return $"El bus {bus.Placa} ya alcanzó su capacidad máxima ({bus.Capacidad} pasajeros).";
                }
            }
            else
            {
                var colegioExistente = await _context.ParadasDb.AnyAsync(p =>
                    p.IdRuta == idRuta &&
                    p.IdAlumno == null &&
                    (!idParadaExcluir.HasValue || p.IdParada != idParadaExcluir.Value));

                if (colegioExistente)
                    return "Ya existe una parada del colegio en esta ruta.";
            }

            return null;
        }

        private async Task AsignarBusAlAlumnoSiAplicaAsync(int idRuta, int? idAlumno)
        {
            if (!idAlumno.HasValue) return;

            var ruta = await _context.RutasDb.FindAsync(idRuta);
            if (ruta == null) return;

            var alumno = await _context.AlumnosDb.FindAsync(idAlumno.Value);
            if (alumno == null || alumno.IdBusAsignado.HasValue) return;

            alumno.IdBusAsignado = ruta.IdBus;
            await _context.SaveChangesAsync();
        }
    }
}
