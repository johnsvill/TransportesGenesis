using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TransportesGenesis.Pages
{
    [Authorize] // Requiere autenticación para acceder a esta página
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // Aquí puedes cargar datos dinámicos para el dashboard
            // Por ejemplo, estadísticas reales de la base de datos
        }
    }
}
