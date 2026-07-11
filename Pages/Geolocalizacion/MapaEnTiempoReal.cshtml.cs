using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Helpers;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Pages.Geolocalizacion
{
    [Authorize(Roles = "Administrador,Piloto,Monitor")]
    public class MapaEnTiempoRealModel : PageModel
    {
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguracionService _configuracionService;

        public string? PadreNombre { get; set; }
        public string ConfigJson { get; set; } = "{}";

        public MapaEnTiempoRealModel(
            IAlumnoRepository alumnoRepository,
            ApplicationDbContext context,
            IConfiguracionService configuracionService)
        {
            _alumnoRepository = alumnoRepository;
            _context = context;
            _configuracionService = configuracionService;
        }

        public async Task OnGetAsync(int? idBus, int? idRuta)
        {
            await CargarNombrePadreAsync();

            var busesElegibles = await MapaTiempoRealQueries.GetBusesElegiblesAsync(_context);
            var colegio = await _configuracionService.ObtenerColegioAsync();

            ConfigJson = JsonSerializer.Serialize(new
            {
                modoSelector = true,
                idBusPreseleccionado = idBus,
                idRutaPreseleccionada = idRuta,
                busesElegibles,
                colegio = new
                {
                    nombre = colegio.Nombre,
                    direccion = colegio.Direccion,
                    latitud = (double)colegio.Latitud,
                    longitud = (double)colegio.Longitud
                }
            }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        private async Task CargarNombrePadreAsync()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var hijos = await _alumnoRepository.GetAlumnosByPadreUserIdAsync(userId);
                    if (hijos != null && hijos.Any())
                    {
                        var primerHijo = hijos.First();
                        if (primerHijo.Padres != null)
                        {
                            PadreNombre = $"{primerHijo.Padres.Nombre} {primerHijo.Padres.Apellido}";
                        }
                    }
                }

                if (string.IsNullOrEmpty(PadreNombre))
                {
                    PadreNombre = User.Identity?.Name;
                }
            }
            catch
            {
                PadreNombre = User.Identity?.Name;
            }
        }
    }
}
