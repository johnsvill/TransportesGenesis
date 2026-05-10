using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.DTOs.Ruta;
using TransportesGenesis.Models.DB.Negocio;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace TransportesGenesis.Pages.Monitor
{
    [Authorize(Roles = "Monitor")]
    public class RegistrarRecogidasModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _context;

        public RegistrarRecogidasModel(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
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
        public List<RegistroRecogida> RegistrosExistentes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int idRuta)
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
                    return Page();
                }

                // Buscar asignación activa del monitor
                var asignacion = await _context.AsignacionesPilotoBusDb
                    .Include(a => a.Bus)
                    .Where(a => a.IdUsuarioPiloto == IdMonitor && a.EsActual)
                    .OrderByDescending(a => a.FechaAsignacion)
                    .FirstOrDefaultAsync();

                if (asignacion == null || asignacion.Bus == null)
                {
                    MensajeError = "No tienes un bus asignado actualmente. Contacta al administrador.";
                    return Page();
                }

                IdBus = asignacion.IdBus;

                // Determinar turno según hora actual
                var horaActual = DateTime.Now.Hour;
                TipoRuta = horaActual < 12 ? "Mañana" : "Tarde";

                Console.WriteLine($"[MONITOR] Cargando ruta #{idRuta} para registrar recogidas");

                // Llamar a la API para obtener los detalles de la ruta
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
                        RutaActiva = JsonSerializer.Deserialize<RutaDto>(
                            apiResponse.Data.ToString()!, 
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );

                        if (RutaActiva != null)
                        {
                            // Cargar registros existentes para esta ruta
                            var idsParadas = RutaActiva.Paradas?.Select(p => p.IdParada).ToList() ?? new List<int>();
                            RegistrosExistentes = await _context.RegistrosRecogidaDb
                                .Where(r => idsParadas.Contains(r.IdParada))
                                .ToListAsync();

                            Console.WriteLine($"[MONITOR] Ruta cargada: {RutaActiva.Paradas?.Count ?? 0} paradas, {RegistrosExistentes.Count} registros previos");
                        }
                    }
                    else
                    {
                        MensajeError = "No se pudo cargar la información de la ruta.";
                    }
                }
                else
                {
                    MensajeError = "Error al conectar con el servidor.";
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error inesperado: {ex.Message}";
                Console.WriteLine($"[MONITOR] Excepción: {ex}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                IdMonitor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                NombreMonitor = User.Identity?.Name ?? "Monitor";

                if (string.IsNullOrEmpty(IdMonitor))
                {
                    MensajeError = "No se pudo identificar al usuario.";
                    return RedirectToPage();
                }

                // Buscar asignación activa para obtener IdBus
                var asignacion = await _context.AsignacionesPilotoBusDb
                    .Where(a => a.IdUsuarioPiloto == IdMonitor && a.EsActual)
                    .OrderByDescending(a => a.FechaAsignacion)
                    .FirstOrDefaultAsync();

                if (asignacion == null)
                {
                    MensajeError = "No tienes un bus asignado.";
                    return RedirectToPage();
                }

                IdBus = asignacion.IdBus;

                // Procesar formulario: buscar todos los checkboxes marcados
                var registrosNuevos = 0;
                var ahora = DateTime.Now;

                foreach (var key in Request.Form.Keys)
                {
                    // Formato esperado: "Registros_{idParada}_{idAlumno}"
                    if (key.StartsWith("Registros_") && Request.Form[key] == "true")
                    {
                        var partes = key.Split('_');
                        if (partes.Length == 3 && 
                            int.TryParse(partes[1], out var idParada) && 
                            int.TryParse(partes[2], out var idAlumno))
                        {
                            // Verificar si ya existe el registro
                            var yaExiste = await _context.RegistrosRecogidaDb
                                .AnyAsync(r => r.IdParada == idParada && r.IdAlumno == idAlumno);

                            if (!yaExiste)
                            {
                                var nuevoRegistro = new RegistroRecogida
                                {
                                    IdParada = idParada,
                                    IdAlumno = idAlumno,
                                    FechaHoraRecogida = ahora,
                                    ConfirmadoPor = IdMonitor,
                                    AlumnoPresente = true,
                                    Activo = 1,
                                    FechaRegistro = ahora
                                    // Nota: Latitud/Longitud pueden agregarse si el navegador captura la ubicación
                                };

                                _context.RegistrosRecogidaDb.Add(nuevoRegistro);
                                registrosNuevos++;
                            }
                        }
                    }
                }

                if (registrosNuevos > 0)
                {
                    await _context.SaveChangesAsync();
                    MensajeExito = $"✅ Se registraron {registrosNuevos} alumno(s) exitosamente.";
                    Console.WriteLine($"[MONITOR] {registrosNuevos} recogidas registradas por {NombreMonitor}");
                }
                else
                {
                    MensajeError = "No se seleccionó ningún alumno o todos ya estaban registrados.";
                }

                // Recargar la página con los datos actualizados
                var idRuta = int.Parse(Request.Form["IdRuta"]!);
                return RedirectToPage(new { idRuta });
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al guardar: {ex.Message}";
                Console.WriteLine($"[MONITOR] Error en POST: {ex}");
                return Page();
            }
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
