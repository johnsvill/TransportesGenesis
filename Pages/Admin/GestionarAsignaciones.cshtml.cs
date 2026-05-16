using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Models.DB.Usuarios;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class GestionarAsignacionesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GestionarAsignacionesModel(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<AppUser> Pilotos { get; set; } = new List<AppUser>();
        public List<AppUser> Monitores { get; set; } = new List<AppUser>();
        public List<Bus> BusesDisponibles { get; set; } = new List<Bus>();
        public List<AsignacionPilotoBus> AsignacionesActivas { get; set; } = new List<AsignacionPilotoBus>();

        [BindProperty]
        public string IdUsuario { get; set; } = string.Empty;

        [BindProperty]
        public int IdBus { get; set; }

        [TempData]
        public string Mensaje { get; set; } = string.Empty;

        [TempData]
        public string TipoMensaje { get; set; } = "info";

        public async Task OnGetAsync()
        {
            await CargarDatosAsync();
        }

        public async Task<IActionResult> OnPostCrearAsignacionAsync()
        {
            if (string.IsNullOrEmpty(IdUsuario) || IdBus == 0)
            {
                Mensaje = "Debe seleccionar un usuario y un bus.";
                TipoMensaje = "danger";
                await CargarDatosAsync();
                return Page();
            }

            // Verificar si ya existe una asignación activa para este bus
            var asignacionExistente = await _context.AsignacionesPilotoBusDb
                .FirstOrDefaultAsync(a => a.IdBus == IdBus && a.EsActual);

            if (asignacionExistente != null)
            {
                Mensaje = $"El bus ya tiene una asignación activa. Debe finalizarla primero.";
                TipoMensaje = "warning";
                await CargarDatosAsync();
                return Page();
            }

            // Crear nueva asignación
            var nuevaAsignacion = new AsignacionPilotoBus
            {
                IdUsuarioPiloto = IdUsuario,
                IdBus = IdBus,
                FechaAsignacion = DateTime.Now,
                EsActual = true,
                Activo = 1,
                FechaRegistro = DateTime.Now
            };

            _context.AsignacionesPilotoBusDb.Add(nuevaAsignacion);
            await _context.SaveChangesAsync();

            Mensaje = "Asignación creada exitosamente.";
            TipoMensaje = "success";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostFinalizarAsignacionAsync(int idAsignacion)
        {
            var asignacion = await _context.AsignacionesPilotoBusDb.FindAsync(idAsignacion);
            if (asignacion != null)
            {
                asignacion.EsActual = false;
                asignacion.FechaFinAsignacion = DateTime.Now;
                await _context.SaveChangesAsync();

                Mensaje = "Asignación finalizada exitosamente.";
                TipoMensaje = "success";
            }

            return RedirectToPage();
        }

        private async Task CargarDatosAsync()
        {
            // Obtener todos los usuarios
            var todosLosUsuarios = await _userManager.Users.ToListAsync();

            // Filtrar pilotos y monitores
            foreach (var usuario in todosLosUsuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                if (roles.Contains("Piloto"))
                {
                    Pilotos.Add(usuario);
                }
                if (roles.Contains("Monitor"))
                {
                    Monitores.Add(usuario);
                }
            }

            // Obtener buses disponibles
            BusesDisponibles = await _context.BusesDb
                .Where(b => b.Estado && b.Activo == 1)
                .ToListAsync();

            // Obtener asignaciones activas
            AsignacionesActivas = await _context.AsignacionesPilotoBusDb
                .Include(a => a.Bus)
                .Where(a => a.EsActual)
                .ToListAsync();
        }
    }
}
