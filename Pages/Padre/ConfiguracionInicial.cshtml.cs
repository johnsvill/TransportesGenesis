using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Models.DB.Usuarios;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace TransportesGenesis.Pages.Padre
{
    [Authorize(Roles = "PadreDeFamilia")]
    public class ConfiguracionInicialModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly ApplicationDbContext _context;

        public ConfiguracionInicialModel(
            UserManager<AppUser> userManager,
            IAlumnoRepository alumnoRepository,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _alumnoRepository = alumnoRepository;
            _context = context;
        }

        [BindProperty]
        public string Direccion { get; set; }

        [BindProperty]
        public double Latitud { get; set; }

        [BindProperty]
        public double Longitud { get; set; }

        [BindProperty]
        public int IdAlumno { get; set; }

        public List<Alumnos> Alumnos { get; set; }
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Verificar si ya tiene dirección configurada
            var yaConfigurado = await VerificarConfiguracionCompleta(user.Id);
            if (yaConfigurado)
            {
                // Ya está configurado, redirigir al dashboard
                return RedirectToAction("Index", "PagosPadresFamilia");
            }

            // Obtener alumnos del padre
            Alumnos = await _alumnoRepository.GetAlumnosByPadreUserIdAsync(user.Id);

            if (Alumnos == null || !Alumnos.Any())
            {
                Mensaje = "No tienes alumnos asignados. Por favor contacta al administrador.";
                TipoMensaje = "warning";
                return Page();
            }

            // Si solo tiene un alumno, seleccionarlo automáticamente
            if (Alumnos.Count == 1)
            {
                IdAlumno = Alumnos[0].IdAlumno;
                Console.WriteLine($"[DEBUG] OnGetAsync - Auto-seleccionando alumno único: IdAlumno={IdAlumno}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Debug: Ver qué valores llegan
            Console.WriteLine("========== INICIO POST ConfiguracionInicial ==========");
            Console.WriteLine($"[DEBUG] IdAlumno: {IdAlumno}");
            Console.WriteLine($"[DEBUG] Direccion: '{Direccion}'");
            Console.WriteLine($"[DEBUG] Latitud: {Latitud}");
            Console.WriteLine($"[DEBUG] Longitud: {Longitud}");
            Console.WriteLine($"[DEBUG] ModelState.IsValid: {ModelState.IsValid}");
            Console.WriteLine($"[DEBUG] ModelState.ErrorCount: {ModelState.ErrorCount}");

            if (!ModelState.IsValid)
            {
                // Debug: Ver errores de validación
                Console.WriteLine("[DEBUG] Errores de ModelState:");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Count > 0)
                    {
                        Console.WriteLine($"  - Campo '{key}':");
                        foreach (var error in state.Errors)
                        {
                            Console.WriteLine($"    * {error.ErrorMessage}");
                            if (error.Exception != null)
                            {
                                Console.WriteLine($"    * Exception: {error.Exception.Message}");
                            }
                        }
                    }
                }
                Console.WriteLine("========== FIN POST (ModelState inválido) ==========");

                TipoMensaje = "danger";
                Mensaje = "Por favor completa todos los campos.";
                Alumnos = await CargarAlumnos();
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Direccion))
            {
                TipoMensaje = "danger";
                Mensaje = "La dirección es obligatoria.";
                Alumnos = await CargarAlumnos();
                return Page();
            }

            if (Latitud == 0 || Longitud == 0)
            {
                TipoMensaje = "danger";
                Mensaje = "Por favor usa el botón 'Buscar en Mapa' para obtener las coordenadas de tu dirección.";
                Alumnos = await CargarAlumnos();
                return Page();
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);

                // Actualizar coordenadas del alumno
                var alumno = await _alumnoRepository.GetByIdAsync(IdAlumno);
                if (alumno == null)
                {
                    TipoMensaje = "danger";
                    Mensaje = "Alumno no encontrado.";
                    Alumnos = await CargarAlumnos();
                    return Page();
                }

                alumno.Latitud = (decimal)Latitud;
                alumno.Longitud = (decimal)Longitud;
                await _alumnoRepository.UpdateAsync(alumno);

                // Crear parada automáticamente si el alumno tiene bus asignado
                if (alumno.IdBusAsignado.HasValue)
                {
                    await CrearParadaAutomatica(alumno, Direccion, (decimal)Latitud, (decimal)Longitud);
                }

                // Marcar configuración como completa
                await MarcarConfiguracionCompleta(user.Id);

                TipoMensaje = "success";
                Mensaje = "¡Configuración completada! Tu dirección ha sido registrada exitosamente.";

                // Redirigir al dashboard del padre (PagosPadresFamilia)
                return RedirectToAction("Index", "PagosPadresFamilia");
            }
            catch (Exception ex)
            {
                TipoMensaje = "danger";
                Mensaje = $"Error al guardar la configuración: {ex.Message}";
                Alumnos = await CargarAlumnos();
                return Page();
            }
        }

        private async Task<bool> VerificarConfiguracionCompleta(string userId)
        {
            var alumnos = await _alumnoRepository.GetAlumnosByPadreUserIdAsync(userId);

            // Si todos los alumnos tienen coordenadas, está configurado
            return alumnos.All(a => a.Latitud.HasValue && a.Longitud.HasValue && 
                                    a.Latitud != 0 && a.Longitud != 0);
        }

        private async Task MarcarConfiguracionCompleta(string userId)
        {
            // Agregar un claim
            var user = await _userManager.FindByIdAsync(userId);
            var claims = await _userManager.GetClaimsAsync(user);

            var configuracionClaim = claims.FirstOrDefault(c => c.Type == "ConfiguracionInicial");
            if (configuracionClaim == null)
            {
                await _userManager.AddClaimAsync(user, 
                    new System.Security.Claims.Claim("ConfiguracionInicial", "Completada"));
            }
        }

        private async Task CrearParadaAutomatica(Alumnos alumno, string direccion, decimal latitud, decimal longitud)
        {
            try
            {
                // Buscar ruta activa del bus del alumno
                var rutas = await _context.RutasDb
                    .Where(r => r.IdBus == alumno.IdBusAsignado.Value && r.EsActiva)
                    .OrderBy(r => r.HoraInicio)
                    .ToListAsync();

                var rutaActiva = rutas.FirstOrDefault();

                if (rutaActiva != null)
                {
                    // Obtener el último orden de parada
                    var paradas = await _context.ParadasDb
                        .Where(p => p.IdRuta == rutaActiva.IdRuta)
                        .ToListAsync();

                    var ultimoOrden = paradas.Any() ? paradas.Max(p => p.Orden) : 0;

                    // Crear la nueva parada
                    var nuevaParada = new Parada
                    {
                        IdRuta = rutaActiva.IdRuta,
                        IdAlumno = alumno.IdAlumno,
                        Latitud = latitud,
                        Longitud = longitud,
                        Direccion = direccion,
                        Orden = ultimoOrden + 1, // Agregar al final
                        Activo = 1, // 1 = true en int
                        HoraEstimada = null, // Se calculará después
                        Completada = false
                    };

                    _context.ParadasDb.Add(nuevaParada);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log error pero no fallar la operación principal
                Console.WriteLine($"Error al crear parada automática: {ex.Message}");
            }
        }

        private async Task<List<Alumnos>> CargarAlumnos()
        {
            var user = await _userManager.GetUserAsync(User);
            return await _alumnoRepository.GetAlumnosByPadreUserIdAsync(user.Id);
        }
    }
}
