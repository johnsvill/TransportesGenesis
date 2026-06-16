using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TransportesGenesis.DTOs.Ruta;
using TransportesGenesis.Services.Interfaces;
using System.Net.Http;
using System.Text.Json;

namespace TransportesGenesis.Pages.Monitor
{
    [Authorize(Roles = "Monitor")]
    public class MiRutaModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPilotoService _pilotoService;

        public MiRutaModel(IHttpClientFactory httpClientFactory, IPilotoService pilotoService)
        {
            _httpClientFactory = httpClientFactory;
            _pilotoService = pilotoService;
        }

        public RutaDto? RutaActiva { get; set; }
        public int? IdBus { get; set; }
        public string PlacaBus { get; set; } = string.Empty;
        public string TipoRuta { get; set; } = "Mañana";
        public string MensajeError { get; set; } = string.Empty;
        public string MensajeExito { get; set; } = string.Empty;
        public string NombreMonitor { get; set; } = string.Empty;
        public DateTime FechaRuta { get; set; }
        public bool EsFinDeSemana { get; set; }

        public async Task OnGetAsync(string? turno = null)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    MensajeError = "No se pudo identificar al usuario autenticado.";
                    return;
                }

                NombreMonitor = User.Identity?.Name ?? "Monitor";

                var asignacion = await _pilotoService.GetAsignacionActualAsync(userId);
                IdBus = asignacion?.IdBus;
                PlacaBus = asignacion?.Bus?.Placa ?? string.Empty;

                if (!IdBus.HasValue)
                {
                    MensajeError = "No tienes un bus asignado. Por favor contacta al administrador.";
                    return;
                }

                FechaRuta = ObtenerProximaFechaHabil();
                EsFinDeSemana = DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday;

                if (!string.IsNullOrEmpty(turno) &&
                    (turno.Equals("Mañana", StringComparison.OrdinalIgnoreCase) ||
                     turno.Equals("Tarde", StringComparison.OrdinalIgnoreCase)))
                    TipoRuta = turno.Equals("Mañana", StringComparison.OrdinalIgnoreCase) ? "Mañana" : "Tarde";
                else
                    TipoRuta = DateTime.Now.Hour < 12 ? "Mañana" : "Tarde";

                var fechaBusqueda = DateTime.Now.DayOfWeek >= DayOfWeek.Monday && DateTime.Now.DayOfWeek <= DayOfWeek.Friday
                    ? DateTime.Now.Date
                    : FechaRuta;

                Console.WriteLine($"[MONITOR] Usuario: {NombreMonitor}, IdBus: {IdBus}, Buscando ruta: {fechaBusqueda:dd/MM/yyyy}, Turno: {TipoRuta}");

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

                var response = await client.GetAsync($"/api/rutas/bus/{IdBus}/activa?tipoRuta={TipoRuta}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
                    };

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content, jsonOptions);

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        var dataJson = JsonSerializer.Serialize(apiResponse.Data);
                        RutaActiva = JsonSerializer.Deserialize<RutaDto>(dataJson, jsonOptions);

                        Console.WriteLine($"[MONITOR] Ruta encontrada: ID {RutaActiva?.IdRuta}, Paradas: {RutaActiva?.Paradas?.Count ?? 0}");
                    }
                    else
                    {
                        if (EsFinDeSemana)
                            MensajeError = $"Es fin de semana. La próxima ruta será el {FechaRuta:dddd dd/MM/yyyy}.";
                        else
                            MensajeError = apiResponse?.Message ?? "No hay ruta calculada para este bus en el turno actual.";

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

        private DateTime ObtenerProximaFechaHabil()
        {
            var fecha = DateTime.Now.Date;
            while (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
                fecha = fecha.AddDays(1);
            return fecha;
        }

        private class ApiResponse
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public object? Data { get; set; }
        }
    }
}
