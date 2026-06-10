using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TransportesGenesis.Repositories.Interfaces;
using System.Threading.Tasks;

namespace TransportesGenesis.Pages.Geolocalizacion
{
    [Authorize]
    public class MapaEnTiempoRealModel : PageModel
    {
        private readonly IAlumnoRepository _alumnoRepository;

        public string? PadreNombre { get; set; }

        public MapaEnTiempoRealModel(IAlumnoRepository alumnoRepository)
        {
            _alumnoRepository = alumnoRepository;
        }

        public async Task OnGetAsync()
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
                // Fallback al nombre del usuario si no se obtuvo desde la relación Padres
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
