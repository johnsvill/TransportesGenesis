using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TransportesGenesis.Pages.Geolocalizacion
{
    [Authorize]
    public class SimulacionMapaModel : PageModel
    {
        public void OnGet()
        {
            // Página de simulación de mapa con buses en movimiento
            ViewData["Title"] = "Simulación de Mapa - Transportes Genesis";
        }
    }
}