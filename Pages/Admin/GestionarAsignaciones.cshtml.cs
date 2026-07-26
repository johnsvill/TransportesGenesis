using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GestionarAsignacionesModel> _logger;

        public GestionarAsignacionesModel(
            ApplicationDbContext context, 
            UserManager<AppUser> userManager,
            ILogger<GestionarAsignacionesModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public List<AppUser> Pilotos { get; set; } = new List<AppUser>();
        public List<AppUser> Monitores { get; set; } = new List<AppUser>();
        public List<AppUser> TodosLosUsuarios { get; set; } = new List<AppUser>(); // Para mostrar en tabla activas
        public List<Bus> BusesDisponibles { get; set; } = new List<Bus>();
        public List<AsignacionPilotoBus> AsignacionesActivas { get; set; } = new List<AsignacionPilotoBus>();
        public Dictionary<string, string> RolPorUsuarioId { get; set; } = new();

        // NO usar [BindProperty] para evitar binding en handlers que no los necesitan

        [TempData]
        public string Mensaje { get; set; } = string.Empty;

        [TempData]
        public string TipoMensaje { get; set; } = "info";

        public async Task OnGetAsync()
        {
            await CargarDatosAsync();
        }

        public async Task<IActionResult> OnPostAsync(
            [FromForm] string? action,
            [FromForm] int idAsignacion,
            [FromForm] string? IdUsuario, 
            [FromForm] int IdBus)
        {
            // ======== LOGGING EXTREMO ========
            Console.WriteLine("========================================");
            Console.WriteLine("POST RECIBIDO EN OnPostAsync");
            Console.WriteLine($"Action: {action}");
            Console.WriteLine($"idAsignacion: {idAsignacion}");
            Console.WriteLine($"IdUsuario: {IdUsuario}");
            Console.WriteLine($"IdBus: {IdBus}");
            Console.WriteLine("========================================");

            _logger.LogInformation("=== INICIO POST ===");
            _logger.LogInformation($"Action: {action}");
            _logger.LogInformation($"idAsignacion: {idAsignacion}");
            _logger.LogInformation($"IdUsuario: '{IdUsuario}'");
            _logger.LogInformation($"IdBus: {IdBus}");

            // Si la acción es "finalizar", delegar al método de finalización
            if (action == "finalizar")
            {
                return await FinalizarAsignacionAsync(idAsignacion);
            }

            // Si no hay acción o la acción es diferente, es una creación
            return await CrearAsignacionAsync(IdUsuario, IdBus);
        }

        private async Task<IActionResult> CrearAsignacionAsync(string? IdUsuario, int IdBus)
        {
            _logger.LogInformation("=== Ejecutando CrearAsignacionAsync ===");

            // VALIDACIÓN: Solo para creación de asignaciones
            if (string.IsNullOrEmpty(IdUsuario) || IdBus == 0)
            {
                _logger.LogWarning("Validación falló: Usuario o Bus no seleccionado");
                Mensaje = "Debe seleccionar un usuario y un bus.";
                TipoMensaje = "danger";
                await CargarDatosAsync();
                return Page();
            }

            try
            {
                _logger.LogInformation($"Verificando asignación existente para usuario: {IdUsuario}");

                // VALIDACIÓN 1: Verificar si el usuario ya tiene una asignación activa
                var usuarioTieneAsignacion = await _context.AsignacionesPilotoBusDb
                    .FirstOrDefaultAsync(a => a.IdUsuarioPiloto == IdUsuario && a.EsActual);

                if (usuarioTieneAsignacion != null)
                {
                    var usuario = await _userManager.FindByIdAsync(IdUsuario);
                    _logger.LogWarning($"Usuario {usuario?.UserName} ya tiene asignación activa al Bus #{usuarioTieneAsignacion.IdBus}");
                    Mensaje = $"El usuario {usuario?.UserName} ya tiene una asignación activa al Bus #{usuarioTieneAsignacion.IdBus}. Debe finalizarla primero.";
                    TipoMensaje = "warning";
                    await CargarDatosAsync();
                    return Page();
                }

                _logger.LogInformation($"Verificando asignación existente para bus: {IdBus}");

                // VALIDACIÓN 2: Verificar que el usuario existe (necesario antes de validar roles)
                var usuarioExiste = await _userManager.FindByIdAsync(IdUsuario);
                if (usuarioExiste == null)
                {
                    _logger.LogError($"Usuario con ID {IdUsuario} no existe en la base de datos");
                    Mensaje = $"❌ Error: El usuario seleccionado no existe.";
                    TipoMensaje = "danger";
                    await CargarDatosAsync();
                    return Page();
                }

                // VALIDACIÓN 3: Verificar que el bus no tenga ya alguien con el mismo rol
                var asignacionesBus = await _context.AsignacionesPilotoBusDb
                    .Where(a => a.IdBus == IdBus && a.EsActual)
                    .ToListAsync();

                var rolesUsuarioNuevo = await _userManager.GetRolesAsync(usuarioExiste);
                var esPiloto = rolesUsuarioNuevo.Contains("Piloto");
                var esMonitor = rolesUsuarioNuevo.Contains("Monitor");

                foreach (var asig in asignacionesBus)
                {
                    var userAsig = await _userManager.FindByIdAsync(asig.IdUsuarioPiloto);
                    if (userAsig == null) continue;
                    var rolesAsig = await _userManager.GetRolesAsync(userAsig);

                    if (esPiloto && rolesAsig.Contains("Piloto"))
                    {
                        Mensaje = $"El bus #{IdBus} ya tiene un piloto asignado ({userAsig.UserName}). Finalice esa asignación primero.";
                        TipoMensaje = "warning";
                        await CargarDatosAsync();
                        return Page();
                    }
                    if (esMonitor && rolesAsig.Contains("Monitor"))
                    {
                        Mensaje = $"El bus #{IdBus} ya tiene un monitor asignado ({userAsig.UserName}). Finalice esa asignación primero.";
                        TipoMensaje = "warning";
                        await CargarDatosAsync();
                        return Page();
                    }
                }

                // Un bus puede tener hasta 2 asignaciones activas: 1 piloto + 1 monitor
                if (asignacionesBus.Count >= 2)
                {
                    Mensaje = $"El bus #{IdBus} ya tiene piloto y monitor asignados. Finalice una asignación para reasignar.";
                    TipoMensaje = "warning";
                    await CargarDatosAsync();
                    return Page();
                }

                // SOLUCIÓN TEMPORAL: Eliminar asignaciones inactivas del bus para evitar conflicto con índice único
                var asignacionesInactivasBus = await _context.AsignacionesPilotoBusDb
                    .Where(a => a.IdBus == IdBus && !a.EsActual)
                    .ToListAsync();

                if (asignacionesInactivasBus.Any())
                {
                    _logger.LogWarning($"Eliminando {asignacionesInactivasBus.Count} asignaciones inactivas del bus #{IdBus} por restricción de índice único");
                    _context.AsignacionesPilotoBusDb.RemoveRange(asignacionesInactivasBus);
                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation("Validaciones pasadas, creando asignación...");

                // VALIDACIÓN 4: Verificar que el bus existe
                var busExiste = await _context.BusesDb.FindAsync(IdBus);
                if (busExiste == null)
                {
                    _logger.LogError($"Bus con ID {IdBus} no existe en la base de datos");
                    Mensaje = $"❌ Error: El bus seleccionado no existe.";
                    TipoMensaje = "danger";
                    await CargarDatosAsync();
                    return Page();
                }

                _logger.LogInformation($"Usuario válido: {usuarioExiste.UserName}, Bus válido: {busExiste.Placa}");

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

                _logger.LogInformation($"Agregando asignación: {usuarioExiste.UserName} → Bus {busExiste.Placa}");
                _context.AsignacionesPilotoBusDb.Add(nuevaAsignacion);

                _logger.LogInformation("Guardando cambios en la base de datos...");
                await _context.SaveChangesAsync();

                _logger.LogInformation($"✅ Asignación creada exitosamente: {usuarioExiste.UserName} → Bus #{IdBus}");
                Mensaje = $"✅ Asignación creada exitosamente: {usuarioExiste.UserName} → Bus {busExiste.Placa}";
                TipoMensaje = "success";

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al crear asignación. IdUsuario: {IdUsuario}, IdBus: {IdBus}");

                // Log detallado de la excepción completa
                var innerException = ex.InnerException;
                var errorDetails = ex.Message;

                if (innerException != null)
                {
                    errorDetails += $"\n\nInner Exception: {innerException.Message}";
                    _logger.LogError($"Inner Exception: {innerException.Message}");

                    if (innerException.InnerException != null)
                    {
                        errorDetails += $"\n\nInner Inner Exception: {innerException.InnerException.Message}";
                        _logger.LogError($"Inner Inner Exception: {innerException.InnerException.Message}");
                    }
                }

                Mensaje = $"❌ Error al crear la asignación: {errorDetails}";
                TipoMensaje = "danger";
                await CargarDatosAsync();
                return Page();
            }
        }

        private async Task<IActionResult> FinalizarAsignacionAsync(int idAsignacion)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("EJECUTANDO FinalizarAsignacionAsync");
            Console.WriteLine($"idAsignacion: {idAsignacion}");
            Console.WriteLine("========================================");

            _logger.LogInformation($"=== Ejecutando FinalizarAsignacionAsync === IdAsignacion: {idAsignacion}");

            try
            {
                // Validar que el ID no sea 0
                if (idAsignacion == 0)
                {
                    _logger.LogWarning("ID de asignación inválido (0)");
                    Mensaje = "❌ ID de asignación inválido.";
                    TipoMensaje = "danger";
                    await CargarDatosAsync();
                    return Page();
                }

                // Buscar la asignación
                var asignacion = await _context.AsignacionesPilotoBusDb
                    .Include(a => a.Bus)
                    .FirstOrDefaultAsync(a => a.IdAsignacion == idAsignacion);

                if (asignacion == null)
                {
                    _logger.LogWarning($"Asignación con ID {idAsignacion} no encontrada");
                    Mensaje = $"❌ No se encontró la asignación con ID {idAsignacion}.";
                    TipoMensaje = "danger";
                    await CargarDatosAsync();
                    return Page();
                }

                // Verificar que la asignación esté activa
                if (!asignacion.EsActual)
                {
                    _logger.LogWarning($"Asignación {idAsignacion} ya está finalizada");
                    Mensaje = "⚠️ Esta asignación ya ha sido finalizada.";
                    TipoMensaje = "warning";
                    await CargarDatosAsync();
                    return Page();
                }

                // Finalizar la asignación
                asignacion.EsActual = false;
                asignacion.FechaFinAsignacion = DateTime.Now;
                await _context.SaveChangesAsync();

                var usuario = await _userManager.FindByIdAsync(asignacion.IdUsuarioPiloto);
                _logger.LogInformation($"✅ Asignación finalizada: {usuario?.UserName} → Bus {asignacion.Bus?.Placa}");
                Mensaje = $"✅ Asignación finalizada exitosamente: {usuario?.UserName} → Bus {asignacion.Bus?.Placa}";
                TipoMensaje = "success";

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al finalizar asignación {idAsignacion}");
                Mensaje = $"❌ Error al finalizar la asignación: {ex.Message}";
                TipoMensaje = "danger";
                await CargarDatosAsync();
                return Page();
            }
        }

        private async Task CargarDatosAsync()
        {
            AsignacionesActivas = await _context.AsignacionesPilotoBusDb
                .Include(a => a.Bus)
                .Where(a => a.EsActual)
                .ToListAsync();

            TodosLosUsuarios = await _userManager.Users.ToListAsync();

            RolPorUsuarioId = new Dictionary<string, string>();
            foreach (var u in TodosLosUsuarios)
            {
                var roles = await _userManager.GetRolesAsync(u);
                if (roles.Contains("Piloto"))       RolPorUsuarioId[u.Id] = "Piloto";
                else if (roles.Contains("Monitor")) RolPorUsuarioId[u.Id] = "Monitor";
            }

            var usuariosYaAsignados = AsignacionesActivas.Select(a => a.IdUsuarioPiloto).ToHashSet();

            // Mapear qué roles tiene asignados cada bus
            var rolesPorBus = new Dictionary<int, HashSet<string>>();
            foreach (var asignacion in AsignacionesActivas)
            {
                var usuario = TodosLosUsuarios.FirstOrDefault(u => u.Id == asignacion.IdUsuarioPiloto);
                if (usuario == null) continue;
                var roles = await _userManager.GetRolesAsync(usuario);

                if (!rolesPorBus.ContainsKey(asignacion.IdBus))
                    rolesPorBus[asignacion.IdBus] = new HashSet<string>();

                if (roles.Contains("Piloto"))  rolesPorBus[asignacion.IdBus].Add("Piloto");
                if (roles.Contains("Monitor")) rolesPorBus[asignacion.IdBus].Add("Monitor");
            }

            // Pilotos y monitores sin asignación activa
            foreach (var usuario in TodosLosUsuarios)
            {
                if (usuariosYaAsignados.Contains(usuario.Id)) continue;
                var roles = await _userManager.GetRolesAsync(usuario);
                if (roles.Contains("Piloto"))  Pilotos.Add(usuario);
                if (roles.Contains("Monitor")) Monitores.Add(usuario);
            }

            // Bus disponible si le falta piloto O monitor (desaparece solo cuando tiene ambos)
            var todosLosBuses = await _context.BusesDb
                .Where(b => b.Estado && b.Activo == 1)
                .ToListAsync();

            BusesDisponibles = todosLosBuses.Where(b =>
            {
                if (!rolesPorBus.TryGetValue(b.IdBus, out var roles))
                    return true; // sin asignaciones → disponible

                var tienePiloto  = roles.Contains("Piloto");
                var tieneMonitor = roles.Contains("Monitor");
                return !tienePiloto || !tieneMonitor;
            }).ToList();
        }
    }
}
