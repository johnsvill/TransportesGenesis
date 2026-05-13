using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using System.Text.Json;

namespace TransportesGenesis.Pages.Monitor
{
    [Authorize(Roles = "Monitor")]
    public class ListadoNinosModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ListadoNinosModel(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public int IdBus { get; set; } = 4; // TODO: Obtener del usuario autenticado
        public string NumeroBus { get; set; } = "001"; // TODO: Obtener de la BD
        public string TipoRuta { get; set; } = "Mañana";
        public string MensajeError { get; set; } = string.Empty;
        public string MensajeExito { get; set; } = string.Empty;
        public List<AlumnoAsistencia> Alumnos { get; set; } = new();
        public int IdRuta { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // Determinar turno según hora actual
                var horaActual = DateTime.Now.Hour;
                TipoRuta = horaActual < 12 ? "Mañana" : "Tarde";

                // Obtener información del bus
                var bus = await _context.Set<Bus>()
                    .FirstOrDefaultAsync(b => b.IdBus == IdBus);

                if (bus != null)
                {
                    NumeroBus = bus.Placa ?? IdBus.ToString("000");
                }

                // Obtener ruta activa del día
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

                var response = await client.GetAsync($"/api/rutas/bus/{IdBus}/activa?tipoRuta={TipoRuta}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    });

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        var rutaDto = JsonSerializer.Deserialize<RutaDto>(
                            apiResponse.Data.ToString()!, 
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );

                        if (rutaDto != null)
                        {
                            IdRuta = rutaDto.IdRuta;

                            // Obtener listado de alumnos de todas las paradas (excluyendo paradas sin alumno, como el colegio)
                            foreach (var parada in rutaDto.Paradas.Where(p => p.IdAlumno.HasValue).OrderBy(p => p.Orden))
                            {
                                // Verificar si ya existe registro de asistencia para hoy
                                var registroExistente = await _context.Set<RegistroRecogida>()
                                    .FirstOrDefaultAsync(r => 
                                        r.Parada.IdParada == parada.IdParada &&
                                        r.FechaHoraRecogida.Date == DateTime.Now.Date);

                                Alumnos.Add(new AlumnoAsistencia
                                {
                                    IdAlumno = parada.IdAlumno.Value, // Ya sabemos que no es null
                                    IdParada = parada.IdParada,
                                    NombreCompleto = parada.NombreAlumno,
                                    Direccion = parada.Direccion,
                                    Orden = parada.Orden,
                                    Presente = registroExistente?.AlumnoPresente ?? false,
                                    IdRegistro = registroExistente?.IdRegistro
                                });
                            }
                        }
                    }
                    else
                    {
                        MensajeError = "No hay ruta calculada para este bus en el turno actual.";
                    }
                }
                else
                {
                    MensajeError = "Error al obtener la ruta del servidor.";
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error inesperado: {ex.Message}";
                Console.WriteLine($"[MONITOR - LISTADO] Excepción: {ex}");
            }
        }

        public async Task<IActionResult> OnPostActualizarAsistenciaAsync([FromBody] ActualizarAsistenciaRequest request)
        {
            try
            {
                var userId = User.Identity?.Name ?? "Sistema";

                // Buscar si ya existe un registro para este alumno en esta parada hoy
                var registroExistente = await _context.Set<RegistroRecogida>()
                    .Include(r => r.Parada)
                    .Include(r => r.Alumno)
                    .FirstOrDefaultAsync(r => 
                        r.Alumno.IdAlumno == request.IdAlumno &&
                        r.Parada.IdParada == request.IdParada &&
                        r.FechaHoraRecogida.Date == DateTime.Now.Date);

                if (registroExistente != null)
                {
                    // Actualizar registro existente
                    registroExistente.AlumnoPresente = request.Presente;
                    registroExistente.FechaHoraRecogida = DateTime.Now;
                    registroExistente.ConfirmadoPor = userId;
                }
                else
                {
                    // Crear nuevo registro
                    var parada = await _context.Set<Parada>()
                        .FirstOrDefaultAsync(p => p.IdParada == request.IdParada);

                    var alumno = await _context.Set<Alumnos>()
                        .FirstOrDefaultAsync(a => a.IdAlumno == request.IdAlumno);

                    if (parada != null && alumno != null)
                    {
                        var nuevoRegistro = new RegistroRecogida
                        {
                            Parada = parada,
                            Alumno = alumno,
                            FechaHoraRecogida = DateTime.Now,
                            AlumnoPresente = request.Presente,
                            ConfirmadoPor = userId,
                            Latitud = parada.Latitud,
                            Longitud = parada.Longitud
                        };

                        _context.Set<RegistroRecogida>().Add(nuevoRegistro);
                    }
                }

                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true, message = "Asistencia actualizada correctamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MONITOR - GUARDAR] Error: {ex}");
                return new JsonResult(new { success = false, message = $"Error al guardar: {ex.Message}" });
            }
        }

        public class AlumnoAsistencia
        {
            public int IdAlumno { get; set; }
            public int IdParada { get; set; }
            public string NombreCompleto { get; set; } = string.Empty;
            public string Direccion { get; set; } = string.Empty;
            public int Orden { get; set; }
            public bool Presente { get; set; }
            public int? IdRegistro { get; set; }
        }

        public class ActualizarAsistenciaRequest
        {
            public int IdAlumno { get; set; }
            public int IdParada { get; set; }
            public bool Presente { get; set; }
        }

        private class ApiResponse
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public object? Data { get; set; }
        }

        private class RutaDto
        {
            public int IdRuta { get; set; }
            public List<ParadaDto> Paradas { get; set; } = new();
        }

        private class ParadaDto
        {
            public int IdParada { get; set; }
            public int? IdAlumno { get; set; } // Nullable para paradas del colegio
            public string NombreAlumno { get; set; } = string.Empty;
            public string Direccion { get; set; } = string.Empty;
            public int Orden { get; set; }
        }
    }
}
