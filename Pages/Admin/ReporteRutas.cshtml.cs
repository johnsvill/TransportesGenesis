using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class ReporteRutasModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReporteRutasModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Ruta> Rutas { get; set; } = new List<Ruta>();

        public async Task OnGetAsync()
        {
            Rutas = await _context.RutasDb
                .Include(r => r.Bus)
                .Include(r => r.ParadasLink)
                .OrderByDescending(r => r.FechaRegistro)
                .ToListAsync();
        }
    }
}
