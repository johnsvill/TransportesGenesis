using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Models.DB.Usuarios;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Pages.Geolocalizacion
{
    [Authorize(Roles = "Administrador")]
    public class BusDetailsModel : PageModel
    {
        private readonly IBusService _busService;
        private readonly IRutaService _rutaService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BusDetailsModel(
            IBusService busService,
            IRutaService rutaService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager)
        {
            _busService = busService;
            _rutaService = rutaService;
            _context = context;
            _userManager = userManager;
        }

        public BusDto? Bus { get; set; }
        public IEnumerable<RutaDto> Rutas { get; set; } = new List<RutaDto>();
        public string? NombrePiloto { get; set; }
        public string? EmailPiloto { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Bus = await _busService.GetBusByIdAsync(id);
            if (Bus == null)
            {
                return RedirectToPage("/Geolocalizacion/BusesIndex");
            }

            Rutas = await _rutaService.GetRutasByBusAsync(id);

            var asignacion = await _context.AsignacionesPilotoBusDb
                .FirstOrDefaultAsync(a => a.IdBus == id && a.EsActual);

            if (asignacion != null)
            {
                var piloto = await _userManager.FindByIdAsync(asignacion.IdUsuarioPiloto);
                if (piloto != null)
                {
                    NombrePiloto = piloto.UserName ?? piloto.Email;
                    EmailPiloto = piloto.Email;
                }
            }

            return Page();
        }
    }
}
