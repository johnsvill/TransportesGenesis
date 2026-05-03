using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Models.DB.Usuarios;

namespace TransportesGenesis.Controllers
{
    public class DiagnosticoController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DiagnosticoController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Diagnostico
        public async Task<IActionResult> Index()
        {
            var info = new
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalRoles = await _roleManager.Roles.CountAsync(),
                Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync(),
                AdminUser = await _userManager.FindByNameAsync("admin"),
                AdminUserByEmail = await _userManager.FindByEmailAsync("admin@transportesgenesis.com"),
                Users = await _userManager.Users.Select(u => new { 
                    u.UserName, 
                    u.Email, 
                    u.Id,
                    u.EmailConfirmed,
                    u.IsFirstLogin,
                    u.LastLoginDate
                }).ToListAsync()
            };

            return Json(info);
        }

        // GET: /Diagnostico/CrearAdminSiNoExiste
        public async Task<IActionResult> CrearAdminSiNoExiste()
        {
            try
            {
                var adminUser = await _userManager.FindByNameAsync("admin");
                if (adminUser == null)
                {
                    // Crear admin
                    adminUser = new AppUser
                    {
                        UserName = "admin",
                        Email = "admin@transportesgenesis.com",
                        EmailConfirmed = true,
                        IsFirstLogin = false,
                        LastLoginDate = DateTime.Now
                    };

                    var result = await _userManager.CreateAsync(adminUser, "Admin123!");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(adminUser, "Administrador");
                        return Json(new { success = true, message = "Admin creado exitosamente", user = adminUser.UserName });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error al crear admin", errors = result.Errors });
                    }
                }
                else
                {
                    return Json(new { success = true, message = "Admin ya existe", user = adminUser.UserName, email = adminUser.Email });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: /Diagnostico/VerificarCredenciales?usuario=admin&password=Admin123!
        public async Task<IActionResult> VerificarCredenciales(string usuario, string password)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(usuario);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(usuario);
                }

                if (user == null)
                {
                    return Json(new { success = false, message = "Usuario no encontrado" });
                }

                var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
                var roles = await _userManager.GetRolesAsync(user);

                return Json(new 
                { 
                    success = true, 
                    usuarioEncontrado = true,
                    passwordCorrecta = isValidPassword,
                    usuario = user.UserName,
                    email = user.Email,
                    roles = roles,
                    confirmado = user.EmailConfirmed
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: /Diagnostico/CrearUsuarioPrueba
        public async Task<IActionResult> CrearUsuarioPrueba()
        {
            try
            {
                var usuarioPrueba = "padre.test";
                var emailPrueba = "padre@test.com";
                var passwordPrueba = "123456";

                // Verificar si ya existe
                var existingUser = await _userManager.FindByNameAsync(usuarioPrueba);
                if (existingUser != null)
                {
                    return Json(new { success = true, message = "Usuario de prueba ya existe", usuario = existingUser.UserName });
                }

                // Crear usuario padre de prueba
                var user = new AppUser
                {
                    UserName = usuarioPrueba,
                    Email = emailPrueba,
                    EmailConfirmed = true,
                    IsFirstLogin = false,
                    LastLoginDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, passwordPrueba);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "PadreDeFamilia");
                    return Json(new { 
                        success = true, 
                        message = "Usuario padre de prueba creado exitosamente", 
                        usuario = user.UserName,
                        email = user.Email,
                        password = passwordPrueba 
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Error al crear usuario padre", errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: /Diagnostico/ListarTodosLosUsuarios
        public async Task<IActionResult> ListarTodosLosUsuarios()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            var usuariosConRoles = new List<object>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                usuariosConRoles.Add(new
                {
                    usuario.Id,
                    usuario.UserName,
                    usuario.Email,
                    usuario.EmailConfirmed,
                    usuario.IsFirstLogin,
                    usuario.LastLoginDate,
                    Roles = roles
                });
            }

            return Json(new { success = true, usuarios = usuariosConRoles, total = usuariosConRoles.Count });
        }

        // GET: /Diagnostico/VerificarRutasPadres
        public IActionResult VerificarRutasPadres()
        {
            var rutas = new
            {
                Dashboard = "/PagosPadresFamilia",
                Pagos = "/PagosPadresFamilia/Pagos",
                Historial = "/PagosPadresFamilia/Historial",
                ConfirmarAsistencia = "/Padres/ConfirmarAsistencia",
                Traslados = "/Padres/Traslados",
                RutaBus = "/Padres/DashboardRutaBusAsignado",
                MapaGeneral = "/Geolocalizacion/MapaEnTiempoReal",
                EstadoSistema = new
                {
                    UsuarioAutenticado = User.Identity?.IsAuthenticated ?? false,
                    NombreUsuario = User.Identity?.Name,
                    EsPadreDeFamilia = User.IsInRole("PadreDeFamilia"),
                    Roles = User.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                                     .Select(c => c.Value).ToList()
                }
            };

            return Json(rutas);
        }
    }
}