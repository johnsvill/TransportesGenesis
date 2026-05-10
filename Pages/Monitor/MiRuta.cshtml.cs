using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.DTOs.Ruta;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace TransportesGenesis.Pages.Monitor
{
    [Authorize(Roles = "Monitor")]
    public class MiRutaModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _context;

        public MiRutaModel(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        public RutaDto? RutaActiva { get; set; }
        public int IdBus { get; set; }
        public string TipoRuta { get; set; } = "Mañana";
        public string MensajeError { get; set; } = string.Empty;
        public string MensajeExito { get; set; } = string.Empty;
        public string NombreMonitor { get; set; } = string.Empty;
        public string IdMonitor { get; set; } = string.Empty;
        public DateTime FechaRuta { get; set; }
        public bool EsFinDeSemana { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // ========================================
                // LOOKUP DINÁMICO DEL BUS ASIGNADO
                // ========================================
                IdMonitor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                NombreMonitor = User.Identity?.Name ?? "Monitor";

                if (string.IsNullOrEmpty(IdMonitor))
                {
                    MensajeError = "No se pudo identificar al usuario. Por favor, cierra sesión e intenta de nuevo.";
                    return;
                }

                // Buscar asignación activa del monitor (reutilizando AsignacionPilotoBus)
                var asignacion = await _context.AsignacionesPilotoBusDb
                    .Include(a => a.Bus)
                    .Where(a => a.IdUsuarioPiloto == IdMonitor && a.EsActual)
                    .OrderByDescending(a => a.FechaAsignacion)
                    .FirstOrDefaultAsync();

                if (asignacion == null || asignacion.Bus == null)
                {
                    MensajeError = "No tienes un bus asignado actualmente. Contacta al administrador.";
                    return;
                }

                IdBus = asignacion.IdBus;
                Console.WriteLine($"[MONITOR] Usuario {NombreMonitor} ({IdMonitor}) asignado al Bus #{IdBus}");

                // Obtener próxima fecha hábil (omite fines de semana)
                FechaRuta = ObtenerProximaFechaHabil();
                EsFinDeSemana = DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday;

                // Determinar turno según hora actual
                var horaActual = DateTime.Now.Hour;
                TipoRuta = horaActual < 12 ? "Mañana" : "Tarde";

                // Si es día hábil (lunes-viernes), buscar ruta de HOY
                // Si es fin de semana, buscar ruta del próximo lunes
                var fechaBusqueda = DateTime.Now.DayOfWeek >= DayOfWeek.Monday && DateTime.Now.DayOfWeek <= DayOfWeek.Friday
                    ? DateTime.Now.Date
                    : FechaRuta;

                Console.WriteLine($"[MONITOR] Buscando ruta para: {fechaBusqueda:dd/MM/yyyy}, Turno: {TipoRuta}");

                // Llamar a la API para obtener la ruta activa
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

                // IMPORTANTE: Buscar rutas del día calculado (no de "hoy" si es fin de semana)
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
                        RutaActiva = JsonSerializer.Deserialize<RutaDto>(
                            apiResponse.Data.ToString()!, 
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );

                        Console.WriteLine($"[MONITOR] Ruta encontrada: ID {RutaActiva?.IdRuta}, Paradas: {RutaActiva?.Paradas?.Count ?? 0}");
                    }
                    else
                    {
                        if (EsFinDeSemana)
                        {
                            MensajeError = $"Es fin de semana. La próxima ruta será el {FechaRuta:dddd dd/MM/yyyy}.";
                        }
                        else
                        {
                            MensajeError = apiResponse?.Message ?? "No hay ruta calculada para este bus en el turno actual. Solicita al administrador que calcule las rutas del día.";
                        }
                        Console.WriteLine($"[MONITOR] Sin ruta: {MensajeError}");
                    }
                }
                else
                {
                    MensajeError = "Error al obtener la ruta del servidor.";
                    Console.WriteLine($"[MONITOR] Error HTTP: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error inesperado: {ex.Message}";
                Console.WriteLine($"[MONITOR] Excepción: {ex}");
            }
        }

        /// <summary>
        /// Obtiene la próxima fecha hábil (omite sábados y domingos)
        /// </summary>
        private DateTime ObtenerProximaFechaHabil()
        {
            var fecha = DateTime.Now.Date;

            // Si es fin de semana, avanzar al próximo lunes
            while (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                fecha = fecha.AddDays(1);
            }

            return fecha;
        }

        // Clase auxiliar para deserializar respuesta de API
        private class ApiResponse
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public object? Data { get; set; }
        }
    }
}
