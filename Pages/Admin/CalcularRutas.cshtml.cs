using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TransportesGenesis.DTOs.Ruta;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class CalcularRutasModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CalcularRutasModel> _logger;

        public CalcularRutasModel(IHttpClientFactory httpClientFactory, ILogger<CalcularRutasModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [BindProperty]
        public DateTime FechaCalculo { get; set; } = DateTime.Today.AddDays(1); // Por defecto mañana

        [BindProperty]
        public string TipoRuta { get; set; } = "Mañana";

        public List<ResultadoCalculoBus>? ResultadosCalculo { get; set; }
        public string? MensajeError { get; set; }
        public bool CalculoCompletado { get; set; }
        public int TotalBusesProcesados { get; set; }
        public int TotalParadasGeneradas { get; set; }

        public void OnGet()
        {
            // Inicialización
            _logger.LogInformation("[ADMIN CALCULAR] Página cargada");
        }

        public async Task<IActionResult> OnPostCalcularRutasAsync()
        {
            try
            {
                _logger.LogInformation($"[ADMIN CALCULAR] Iniciando cálculo masivo para {FechaCalculo:yyyy-MM-dd} - Turno: {TipoRuta}");

                // Validar que no sea fin de semana
                if (FechaCalculo.DayOfWeek == DayOfWeek.Saturday || FechaCalculo.DayOfWeek == DayOfWeek.Sunday)
                {
                    MensajeError = "No se pueden calcular rutas para sábados o domingos. Por favor seleccione un día hábil (lunes a viernes).";
                    _logger.LogWarning($"[ADMIN CALCULAR] Intento de calcular ruta en fin de semana: {FechaCalculo:yyyy-MM-dd}");
                    return Page();
                }

                // Lista de buses a procesar (hardcoded por ahora, TODO: obtener desde BD)
                var idsBuses = new List<int> { 1, 2, 3, 4 };

                ResultadosCalculo = new List<ResultadoCalculoBus>();

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

                foreach (var idBus in idsBuses)
                {
                    var resultado = new ResultadoCalculoBus
                    {
                        IdBus = idBus,
                        PlacaBus = $"Bus {idBus}", // TODO: Obtener placa real desde BD
                        Exitoso = false
                    };

                    try
                    {
                        _logger.LogInformation($"[ADMIN CALCULAR] Procesando Bus {idBus}...");

                        // Preparar DTO para calcular ruta
                        var calcularDto = new CalcularRutaDto
                        {
                            IdBus = idBus,
                            Fecha = FechaCalculo,
                            TipoRuta = TipoRuta
                        };

                        var jsonContent = JsonSerializer.Serialize(calcularDto);
                        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                        // Llamar al endpoint de cálculo
                        var response = await httpClient.PostAsync("/api/rutas/calcular", content);
                        var responseBody = await response.Content.ReadAsStringAsync();

                        _logger.LogInformation($"[ADMIN CALCULAR] Bus {idBus} - Response Status: {response.StatusCode}");
                        _logger.LogInformation($"[ADMIN CALCULAR] Bus {idBus} - Response Body: {responseBody}");

                        if (response.IsSuccessStatusCode)
                        {
                            var apiResponse = JsonSerializer.Deserialize<ApiResponse<RutaDto>>(responseBody, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                            if (apiResponse?.Success == true && apiResponse.Data != null)
                            {
                                resultado.Exitoso = true;
                                resultado.IdRuta = apiResponse.Data.IdRuta;
                                resultado.CantidadParadas = apiResponse.Data.Paradas?.Count ?? 0;
                                resultado.HoraInicio = apiResponse.Data.HoraInicio.ToString(@"hh\:mm");
                                resultado.TiempoEstimado = CalcularTiempoTotal(apiResponse.Data.Paradas);
                                resultado.Mensaje = $"Ruta calculada exitosamente: {resultado.CantidadParadas} paradas";

                                TotalParadasGeneradas += resultado.CantidadParadas;
                                _logger.LogInformation($"[ADMIN CALCULAR] Bus {idBus} - Éxito: {resultado.CantidadParadas} paradas");
                            }
                            else
                            {
                                resultado.Mensaje = apiResponse?.Message ?? "Error desconocido en la respuesta";
                                _logger.LogWarning($"[ADMIN CALCULAR] Bus {idBus} - API retornó success=false: {resultado.Mensaje}");
                            }
                        }
                        else
                        {
                            resultado.Mensaje = $"Error HTTP {response.StatusCode}: {responseBody}";
                            _logger.LogError($"[ADMIN CALCULAR] Bus {idBus} - Error HTTP: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        resultado.Mensaje = $"Excepción: {ex.Message}";
                        _logger.LogError(ex, $"[ADMIN CALCULAR] Bus {idBus} - Excepción no controlada");
                    }

                    ResultadosCalculo.Add(resultado);
                    if (resultado.Exitoso) TotalBusesProcesados++;
                }

                CalculoCompletado = true;
                _logger.LogInformation($"[ADMIN CALCULAR] Proceso completado: {TotalBusesProcesados}/{idsBuses.Count} buses exitosos, {TotalParadasGeneradas} paradas totales");

                return Page();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error general en el cálculo masivo: {ex.Message}";
                _logger.LogError(ex, "[ADMIN CALCULAR] Error general no controlado");
                return Page();
            }
        }

        private string CalcularTiempoTotal(List<ParadaRutaDto>? paradas)
        {
            if (paradas == null || paradas.Count == 0)
                return "N/A";

            var primeraParada = paradas.OrderBy(p => p.Orden).FirstOrDefault();
            var ultimaParada = paradas.OrderBy(p => p.Orden).LastOrDefault();

            if (primeraParada?.HoraEstimada == null || ultimaParada?.HoraEstimada == null)
                return "N/A";

            var tiempoTotal = ultimaParada.HoraEstimada.Value - primeraParada.HoraEstimada.Value;
            return $"{(int)tiempoTotal.TotalMinutes} min";
        }

        // Clase auxiliar para deserializar respuesta de API
        private class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public T? Data { get; set; }
        }
    }

    public class ResultadoCalculoBus
    {
        public int IdBus { get; set; }
        public string PlacaBus { get; set; } = string.Empty;
        public bool Exitoso { get; set; }
        public int? IdRuta { get; set; }
        public int CantidadParadas { get; set; }
        public string HoraInicio { get; set; } = string.Empty;
        public string TiempoEstimado { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}
