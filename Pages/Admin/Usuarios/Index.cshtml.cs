using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Usuarios;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Pages.Admin.Usuarios
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public List<AppUser> Usuarios { get; set; } = new();
        public Dictionary<string, IList<string>> UsuarioRoles { get; set; } = new();

        [BindProperty]
        public UsuarioInputModel Input { get; set; } = new();

        public string? Mensaje { get; set; }
        public string? TipoMensaje { get; set; }

        public async Task OnGetAsync()
        {
            await CargarUsuarios();
        }

        private async Task CargarUsuarios()
        {
            Usuarios = await _userManager.Users.ToListAsync();

            foreach (var usuario in Usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                UsuarioRoles[usuario.Id] = roles;
            }
        }

        public async Task<IActionResult> OnPostCrearUsuarioAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarUsuarios();
                    return Page();
                }

                // Verificar si el usuario ya existe por nombre
                var existingUserByName = await _userManager.FindByNameAsync(Input.UserName);
                if (existingUserByName != null)
                {
                    Mensaje = "El nombre de usuario ya existe";
                    TipoMensaje = "error";
                    await CargarUsuarios();
                    return Page();
                }

                // Verificar si el usuario ya existe por email
                var existingUserByEmail = await _userManager.FindByEmailAsync(Input.Email);
                if (existingUserByEmail != null)
                {
                    Mensaje = "El correo electrónico ya está registrado";
                    TipoMensaje = "error";
                    await CargarUsuarios();
                    return Page();
                }

                // Crear nuevo usuario
                var user = new AppUser
                {
                    UserName = Input.UserName,
                    Email = Input.Email,
                    EmailConfirmed = true,
                    IsFirstLogin = true,
                    LastLoginDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // Asignar rol
                    var roleResult = await _userManager.AddToRoleAsync(user, Input.Rol);

                    if (roleResult.Succeeded)
                    {
                        // Si es Piloto o Monitor, asignar Bus #1 automáticamente (SOLO PARA PRUEBAS)
                        if (Input.Rol == "Piloto" || Input.Rol == "Monitor")
                        {
                            var bus1 = await _context.BusesDb.FirstOrDefaultAsync(b => b.IdBus == 1);
                            if (bus1 != null)
                            {
                                var asignacion = new AsignacionPilotoBus
                                {
                                    IdUsuarioPiloto = user.Id,
                                    IdBus = 1,
                                    FechaAsignacion = DateTime.Now,
                                    EsActual = true,
                                    Activo = 1,
                                    FechaRegistro = DateTime.Now
                                };

                                _context.AsignacionesPilotoBusDb.Add(asignacion);
                                await _context.SaveChangesAsync();

                                Mensaje = $"Usuario {Input.UserName} creado exitosamente con rol {Input.Rol} y asignado al Bus #1";
                            }
                            else
                            {
                                Mensaje = $"Usuario {Input.UserName} creado con rol {Input.Rol}, pero no se pudo asignar bus (Bus #1 no existe)";
                            }
                        }
                        else
                        {
                            Mensaje = $"Usuario {Input.UserName} creado exitosamente con rol {Input.Rol}";
                        }

                        TipoMensaje = "success";

                        // Limpiar formulario
                        Input = new UsuarioInputModel();
                    }
                    else
                    {
                        // Si falla la asignación de rol, eliminar el usuario creado
                        await _userManager.DeleteAsync(user);
                        Mensaje = "Error al asignar el rol: " + string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        TipoMensaje = "error";
                    }
                }
                else
                {
                    Mensaje = "Error al crear el usuario: " + string.Join(", ", result.Errors.Select(e => e.Description));
                    TipoMensaje = "error";
                }

                await CargarUsuarios();
                return Page();
            }
            catch (Exception ex)
            {
                Mensaje = $"Error inesperado: {ex.Message}";
                TipoMensaje = "error";
                await CargarUsuarios();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostEliminarUsuarioAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && user.UserName != "admin") // No permitir eliminar admin
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    Mensaje = "Usuario eliminado exitosamente";
                    TipoMensaje = "success";
                }
                else
                {
                    Mensaje = "Error al eliminar usuario";
                    TipoMensaje = "error";
                }
            }
            else
            {
                Mensaje = "No se puede eliminar el usuario administrador";
                TipoMensaje = "error";
            }

            await CargarUsuarios();
            return Page();
        }
    }

    public class UsuarioInputModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres")]
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} y máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "El rol es obligatorio")]
        public string Rol { get; set; } = "PadreDeFamilia";
    }
}