using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TransportesGenesis.Pages.Padres
{
    [Authorize(Roles = "PadreDeFamilia")]
    public class TrasladosModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
