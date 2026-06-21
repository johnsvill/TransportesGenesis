using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportesGenesis.Models.DB.Usuarios;
using TransportesGenesis.ViewModels;
using TransportesGenesis.Repositories.Interfaces;

namespace TransportesGenesis.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAlumnoRepository _alumnoRepository;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAlumnoRepository alumnoRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _alumnoRepository = alumnoRepository;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // También intenta encontrar por nombre de usuario
                user = await _userManager.FindByNameAsync(model.Email);
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Usuario no encontrado.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                // Actualizar último login
                user.LastLoginDate = DateTime.Now;
                await _userManager.UpdateAsync(user);

                if (user.IsFirstLogin && !await _userManager.IsInRoleAsync(user, "Administrador"))
                {
                    return RedirectToAction("ForceChangePassword");
                }

                // Redirigir según el rol del usuario
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Administrador"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (roles.Contains("PadreDeFamilia"))
                {
                    return await RedirigirPadreDespuesLoginAsync(user.Id);
                }
                else if (roles.Contains("Piloto"))
                {
                    return RedirectToPage("/Piloto/MiRuta");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Cuenta bloqueada temporalmente.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "Inicio de sesión no permitido.");
            }
            else
            {
                ModelState.AddModelError("", "Credenciales inválidas.");
            }

            return View(model);
        }

        // GET: /Auth/ForceChangePassword
        [HttpGet]
        [Authorize]
        public IActionResult ForceChangePassword()
        {
            return View();
        }

        // POST: /Auth/ForceChangePassword
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ForceChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (result.Succeeded)
            {
                user.IsFirstLogin = false;
                await _userManager.UpdateAsync(user);

                if (await _userManager.IsInRoleAsync(user, "PadreDeFamilia"))
                {
                    return await RedirigirPadreDespuesLoginAsync(user.Id);
                }

                if (await _userManager.IsInRoleAsync(user, "Administrador"))
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // GET: /Auth/Logout
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }

        // POST: /Auth/Logout - Para formularios que usen POST
        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> LogoutPost()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }

        // GET: /Auth/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Redirige al padre: primero configuración de ubicación si falta, luego dashboard principal.
        /// </summary>
        private async Task<IActionResult> RedirigirPadreDespuesLoginAsync(string userId)
        {
            if (await VerificarSiNecesitaConfiguracionInicial(userId))
            {
                return RedirectToPage("/Padre/ConfiguracionInicial");
            }

            return RedirectToAction("Index", "PagosPadresFamilia");
        }

        /// <summary>
        /// Verifica si el padre necesita configurar la dirección de recogida de sus alumnos
        /// </summary>
        private async Task<bool> VerificarSiNecesitaConfiguracionInicial(string userId)
        {
            try
            {
                // Obtener alumnos del padre
                var alumnos = await _alumnoRepository.GetAlumnosByPadreUserIdAsync(userId);

                if (alumnos == null || !alumnos.Any())
                {
                    // Si no tiene alumnos, no necesita configuración
                    return false;
                }

                // Verificar si algún alumno NO tiene coordenadas configuradas
                var necesitaConfiguracion = alumnos.Any(a =>
                    !a.Latitud.HasValue ||
                    !a.Longitud.HasValue ||
                    a.Latitud == 0 ||
                    a.Longitud == 0
                );

                return necesitaConfiguracion;
            }
            catch (Exception)
            {
                // En caso de error, no bloquear el login
                return false;
            }
        }
    }
}
