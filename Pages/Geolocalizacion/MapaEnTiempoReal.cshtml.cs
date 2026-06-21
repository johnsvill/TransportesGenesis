using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Helpers;
using TransportesGenesis.Repositories.Interfaces;

namespace TransportesGenesis.Pages.Geolocalizacion
{
    [Authorize]
    public class MapaEnTiempoRealModel : PageModel
    {
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly ApplicationDbContext _context;

        public string? PadreNombre { get; set; }
        public string ConfigJson { get; set; } = "{}";

        public MapaEnTiempoRealModel(IAlumnoRepository alumnoRepository, ApplicationDbContext context)
        {
            _alumnoRepository = alumnoRepository;
            _context = context;
        }

        public async Task OnGetAsync(int? idBus, int? idRuta)
        {
            await CargarNombrePadreAsync();

            var busesElegibles = await MapaTiempoRealQueries.GetBusesElegiblesAsync(_context);

            ConfigJson = JsonSerializer.Serialize(new
            {
                modoSelector = true,
                idBusPreseleccionado = idBus,
                idRutaPreseleccionada = idRuta,
                busesElegibles
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
