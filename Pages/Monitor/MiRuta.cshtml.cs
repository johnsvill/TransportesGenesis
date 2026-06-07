using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TransportesGenesis.DTOs.Ruta;
using System.Net.Http;
using System.Text.Json;

namespace TransportesGenesis.Pages.Monitor
{
    [Authorize(Roles = "Monitor")]
    public class MiRutaModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MiRutaModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public RutaDto? RutaActiva { get; set; }
        public int IdBus { get; set; } = 4; // TODO: Obtener del usuario autenticado (Claims)
        public string TipoRuta { get; set; } = "Mañana";
        public string MensajeError { get; set; } = string.Empty;
        public string MensajeExito { get; set; } = string.Empty;
        public string NombreMonitor { get; set; } = "Monitor 1"; // TODO: Obtener del usuario autenticado
        public int IdMonitor { get; set; } = 1; // TODO: Obtener del usuario autenticado
        public DateTime FechaRuta { get; set; }
        public bool EsFinDeSemana { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // TODO: En producción, obtener IdBus del monitor autenticado
                // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                // IdBus = await _monitorService.GetBusDelMonitorAsync(userId);

                FechaRuta = ObtenerProximaFechaHabil();
                EsFinDeSemana = DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday;

                var horaActual = DateTime.Now.Hour;
                TipoRuta = horaActual < 12 ? "Mañana" : "Tarde";

                var fechaBusqueda = DateTime.Now.DayOfWeek >= DayOfWeek.Monday && DateTime.Now.DayOfWeek <= DayOfWeek.Friday
                    ? DateTime.Now.Date
                    : FechaRuta;

                Console.WriteLine($"[MONITOR] Buscando ruta para: {fechaBusqueda:dd/MM/yyyy}, Turno: {TipoRuta}");

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
                        {
                            MensajeError = $"Es fin de semana. La próxima ruta será el {FechaRuta:dddd dd/MM/yyyy}.";
                        }
                        else
                        {
                            MensajeError = apiResponse?.Message ?? "No hay ruta calculada para este bus en el turno actual.";
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

        private DateTime ObtenerProximaFechaHabil()
        {
            var fecha = DateTime.Now.Date;

            while (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                fecha = fecha.AddDays(1);
            }

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
