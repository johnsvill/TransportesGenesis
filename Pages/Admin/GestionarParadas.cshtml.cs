using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class GestionarParadasModel : PageModel
    {
        public string? Mensaje { get; set; }
        public string TipoMensaje { get; set; } = "info";

        public void OnGet()
        {
            // La carga de datos se hace vía API desde JavaScript
        }
    }
}
