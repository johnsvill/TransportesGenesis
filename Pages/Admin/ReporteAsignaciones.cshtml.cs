using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Models.DB.Usuarios;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class ReporteAsignacionesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReporteAsignacionesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public class AsignacionDetalle
        {
            public int IdAsignacion { get; set; }
            public string NombreUsuario { get; set; } = string.Empty;
            public string EmailUsuario { get; set; } = string.Empty;
            public string Rol { get; set; } = string.Empty;
            public string PlacaBus { get; set; } = string.Empty;
            public int IdBus { get; set; }
            public DateTime FechaAsignacion { get; set; }
            public DateTime? FechaFinAsignacion { get; set; }
            public bool EsActual { get; set; }
        }

        public List<AsignacionDetalle> Asignaciones { get; set; } = new List<AsignacionDetalle>();

        public async Task OnGetAsync()
        {
            var asignaciones = await _context.AsignacionesPilotoBusDb
                .Include(a => a.Bus)
                .OrderByDescending(a => a.FechaAsignacion)
                .ToListAsync();

            foreach (var asignacion in asignaciones)
            {
                var usuario = await _context.AppUsers.FindAsync(asignacion.IdUsuarioPiloto);
                if (usuario != null)
                {
                    var roles = await _context.UserRoles
                        .Where(ur => ur.UserId == usuario.Id)
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                        .ToListAsync();

                    Asignaciones.Add(new AsignacionDetalle
                    {
                        IdAsignacion = asignacion.IdAsignacion,
                        NombreUsuario = usuario.UserName ?? "Sin nombre",
                        EmailUsuario = usuario.Email ?? "Sin email",
                        Rol = string.Join(", ", roles),
                        PlacaBus = asignacion.Bus?.Placa ?? "Sin placa",
                        IdBus = asignacion.IdBus,
                        FechaAsignacion = asignacion.FechaAsignacion,
                        FechaFinAsignacion = asignacion.FechaFinAsignacion,
                        EsActual = asignacion.EsActual
                    });
                }
            }
        }
    }
}
