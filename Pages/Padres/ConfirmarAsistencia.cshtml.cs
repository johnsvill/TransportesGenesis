using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TransportesGenesis.Pages.Padres
{
    [Authorize(Roles = "PadreDeFamilia")]
    public class ConfirmarAsistenciaModel : PageModel
    {
        public void OnGet()
        {
            // La página se carga y el JavaScript maneja todo
        }
    }
}
