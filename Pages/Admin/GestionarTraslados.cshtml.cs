using Microsoft.AspNetCore.Mvc.RazorPages;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Pages.Admin
{
    public class GestionarTrasladosModel : PageModel
    {
        private readonly ITrasladoService _trasladoService;

        public GestionarTrasladosModel(ITrasladoService trasladoService)
        {
            _trasladoService = trasladoService;
        }

        public void OnGet()
        {
            // La página cargará datos vía JavaScript/API
        }
    }
}
