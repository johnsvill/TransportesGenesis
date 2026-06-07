using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TransportesGenesis.Pages.Geolocalizacion
{
    [Authorize]
    public class MapaEnTiempoRealModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
