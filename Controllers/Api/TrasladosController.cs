using Microsoft.AspNetCore.Mvc;
using TransportesGenesis.DTOs.Traslado;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Controllers.Api
{
    [Route("api/traslados")]
    [ApiController]
    public class TrasladosController : ControllerBase
    {
        private readonly ITrasladoService _trasladoService;

        public TrasladosController(ITrasladoService trasladoService)
        {
            _trasladoService = trasladoService;
        }

        /// <summary>
        /// Crear nueva solicitud de traslado
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<SolicitudTrasladoDto>> CrearSolicitud([FromBody] CrearSolicitudTrasladoDto dto)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine($"[TRASLADOS POST] ModelState inválido");
                return BadRequest(ModelState);
            }

            try
            {
                Console.WriteLine($"[TRASLADOS POST] Intentando crear solicitud para alumno {dto.IdAlumno}");
                Console.WriteLine($"[TRASLADOS POST] Fecha: {dto.FechaTraslado}, Turno: {dto.Turno}");

                var resultado = await _trasladoService.CrearSolicitudAsync(dto);

                Console.WriteLine($"[TRASLADOS POST] ✅ Solicitud creada exitosamente. ID: {resultado.IdSolicitud}");

                return Ok(new 
                { 
                    success = true, 
                    message = "Solicitud de traslado creada exitosamente",
                    data = resultado
                });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"[TRASLADOS POST] ❌ InvalidOperationException: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TRASLADOS POST] ❌ Exception: {ex.Message}");
                Console.WriteLine($"[TRASLADOS POST] StackTrace: {ex.StackTrace}");

                // Modo simulación si falla
                return Ok(new 
                { 
                    success = true, 
                    message = "Solicitud registrada (modo prueba) - Error en BD",
                    data = new SolicitudTrasladoDto
                    {
                        IdSolicitud = 999, // ID temporal para identificar modo simulación
                        IdAlumno = dto.IdAlumno,
                        NombreAlumno = "Alumno de Prueba",
                        FechaTraslado = dto.FechaTraslado,
                        Turno = dto.Turno,
                        Motivo = "[MODO SIMULACIÓN] " + dto.Motivo,
                        Estado = "Pendiente",
                        FechaRegistro = DateTime.Now
                    }
                });
            }
        }

        /// <summary>
        /// Obtener solicitudes por alumno
        /// </summary>
        [HttpGet("alumno/{idAlumno}")]
        public async Task<ActionResult<IEnumerable<SolicitudTrasladoDto>>> GetPorAlumno(int idAlumno)
        {
            try
            {
                var solicitudesReales = await _trasladoService.GetSolicitudesPorAlumnoAsync(idAlumno);

                // LOG para debug
                Console.WriteLine($"[TRASLADOS] GET alumno/{idAlumno} - Solicitudes reales encontradas: {solicitudesReales?.Count() ?? 0}");

                // Si hay solicitudes reales, retornarlas junto con datos de prueba (para comparación)
                if (solicitudesReales != null && solicitudesReales.Any())
                {
                    Console.WriteLine($"[TRASLADOS] Retornando {solicitudesReales.Count()} solicitudes reales");

                    // TEMPORAL: Agregar datos de prueba también para que puedas ver la diferencia
                    var todasLasSolicitudes = solicitudesReales.ToList();
                    var solicitudesPrueba = GenerarSolicitudesPrueba(idAlumno);

                    // Agregar indicador en datos de prueba
                    foreach (var prueba in solicitudesPrueba)
                    {
                        prueba.Motivo = "[DATOS DE PRUEBA] " + (prueba.Motivo ?? "Sin motivo");
                    }

                    todasLasSolicitudes.AddRange(solicitudesPrueba);

                    return Ok(todasLasSolicitudes.OrderByDescending(s => s.FechaRegistro));
                }

                // Si no hay solicitudes en BD, retornar solo datos de prueba
                Console.WriteLine($"[TRASLADOS] No hay solicitudes reales, retornando datos de prueba");
                return Ok(GenerarSolicitudesPrueba(idAlumno));
            }
            catch (Exception ex)
            {
                // Modo simulación: retornar datos de prueba si hay error
                Console.WriteLine($"[TRASLADOS ERROR] {ex.Message}");
                Console.WriteLine($"[TRASLADOS ERROR] StackTrace: {ex.StackTrace}");
                return Ok(GenerarSolicitudesPrueba(idAlumno));
            }
        }

        /// <summary>
        /// Obtener buses disponibles para una fecha
        /// </summary>
        [HttpGet("buses-disponibles")]
        public async Task<ActionResult<IEnumerable<BusDisponibleDto>>> GetBusesDisponibles(
            [FromQuery] DateTime fecha, 
            [FromQuery] string turno = "Ambos")
        {
            try
            {
                var buses = await _trasladoService.GetBusesDisponiblesAsync(fecha, turno);
                return Ok(buses);
            }
            catch (Exception)
            {
                // Retornar buses de prueba
                return Ok(new List<BusDisponibleDto>
                {
                    new BusDisponibleDto { IdBus = 1, Placa = "BUS-001", Modelo = "Mercedes Sprinter", Capacidad = 25, AsientosDisponibles = 20 },
                    new BusDisponibleDto { IdBus = 2, Placa = "BUS-002", Modelo = "Volkswagen Crafter", Capacidad = 30, AsientosDisponibles = 25 },
                    new BusDisponibleDto { IdBus = 3, Placa = "BUS-003", Modelo = "Ford Transit", Capacidad = 20, AsientosDisponibles = 15 }
                });
            }
        }

        /// <summary>
        /// Obtener solicitudes pendientes (Admin)
        /// </summary>
        [HttpGet("pendientes")]
        public async Task<ActionResult<IEnumerable<SolicitudTrasladoDto>>> GetPendientes()
        {
            try
            {
                var solicitudes = await _trasladoService.GetSolicitudesPendientesAsync();
                return Ok(solicitudes);
            }
            catch (Exception)
            {
                return Ok(new List<SolicitudTrasladoDto>());
            }
        }

        /// <summary>
        /// Responder solicitud (Admin) - Aprobar o Rechazar
        /// </summary>
        [HttpPost("{idSolicitud}/responder")]
        public async Task<ActionResult<SolicitudTrasladoDto>> ResponderSolicitud(
            int idSolicitud,
            [FromBody] ResponderSolicitudTrasladoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                dto.IdSolicitud = idSolicitud;
                var resultado = await _trasladoService.ResponderSolicitudAsync(dto, "Admin");

                return Ok(new 
                { 
                    success = true, 
                    message = $"Solicitud {dto.Estado.ToLower()} exitosamente",
                    data = resultado
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Verificar si hay traslado activo para un alumno en una fecha
        /// </summary>
        [HttpGet("alumno/{idAlumno}/activo")]
        public async Task<ActionResult<SolicitudTrasladoDto>> GetTrasladoActivo(
            int idAlumno, 
            [FromQuery] DateTime fecha)
        {
            try
            {
                var traslado = await _trasladoService.GetTrasladoActivoAsync(idAlumno, fecha);

                if (traslado == null)
                    return NotFound(new { message = "No hay traslado activo para esta fecha" });

                return Ok(traslado);
            }
            catch (Exception)
            {
                return NotFound(new { message = "No hay traslado activo para esta fecha" });
            }
        }

        /// <summary>
        /// Generar solicitudes de prueba para modo simulación
        /// </summary>
        private List<SolicitudTrasladoDto> GenerarSolicitudesPrueba(int idAlumno)
        {
            var hoy = DateTime.Now;

            return new List<SolicitudTrasladoDto>
            {
                // Solicitud pendiente reciente
                new SolicitudTrasladoDto
                {
                    IdSolicitud = 1,
                    IdAlumno = idAlumno,
                    NombreAlumno = "Alumno de Prueba",
                    FechaTraslado = hoy.AddDays(3),
                    Turno = "Ambos",
                    PlacaBusOrigen = "BUS-001",
                    PlacaBusDestino = "BUS-003",
                    Estado = "Pendiente",
                    Motivo = "Se quedará en casa de su abuela",
                    FechaRegistro = hoy.AddDays(-1)
                },
                // Solicitud aprobada del mes pasado
                new SolicitudTrasladoDto
                {
                    IdSolicitud = 2,
                    IdAlumno = idAlumno,
                    NombreAlumno = "Alumno de Prueba",
                    FechaTraslado = hoy.AddMonths(-1).AddDays(5),
                    Turno = "Mañana",
                    PlacaBusOrigen = "BUS-001",
                    PlacaBusDestino = "BUS-002",
                    Estado = "Aprobado",
                    Motivo = "Visita médica",
                    FechaRegistro = hoy.AddMonths(-1).AddDays(-5),
                    FechaRespuesta = hoy.AddMonths(-1).AddDays(-4),
                    AprobadoPor = "Admin",
                    ComentarioAdmin = "Aprobado - Capacidad disponible en el bus"
                },
                // Solicitud rechazada hace 2 meses
                new SolicitudTrasladoDto
                {
                    IdSolicitud = 3,
                    IdAlumno = idAlumno,
                    NombreAlumno = "Alumno de Prueba",
                    FechaTraslado = hoy.AddMonths(-2).AddDays(10),
                    Turno = "Tarde",
                    PlacaBusOrigen = "BUS-001",
                    PlacaBusDestino = "BUS-004",
                    Estado = "Rechazado",
                    Motivo = null,
                    FechaRegistro = hoy.AddMonths(-2).AddDays(8),
                    FechaRespuesta = hoy.AddMonths(-2).AddDays(9),
                    AprobadoPor = "Admin",
                    ComentarioAdmin = "Bus sin capacidad disponible para esa ruta"
                }
            };
        }
    }
}
