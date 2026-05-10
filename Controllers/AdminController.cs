using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Models.DB.Usuarios;

namespace TransportesGenesis.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Método temporal para crear usuarios de prueba para Piloto y Monitor
        /// Acceder como Admin a: /Admin/CrearUsuariosPrueba
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CrearUsuariosPrueba()
        {
            try
            {
                var mensajes = new List<string>();

                // ========================================
                // 1. CREAR USUARIO PILOTO
                // ========================================
                var piloto = await _userManager.FindByEmailAsync("piloto1@transportesgenesis.com");
                if (piloto == null)
                {
                    piloto = new AppUser
                    {
                        UserName = "piloto1",
                        Email = "piloto1@transportesgenesis.com",
                        EmailConfirmed = true,
                        IsFirstLogin = false,
                        LastLoginDate = DateTime.Now
                    };

                    var resultPiloto = await _userManager.CreateAsync(piloto, "Piloto123!");
                    if (resultPiloto.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(piloto, "Piloto");
                        mensajes.Add("✅ Usuario Piloto creado exitosamente");

                        // Asignar bus al piloto (asumiendo que existe IdBus = 1)
                        var busExiste = await _context.BusesDb.AnyAsync(b => b.IdBus == 1);
                        if (busExiste)
                        {
                            // Verificar si ya tiene asignación
                            var asignacionExistente = await _context.AsignacionesPilotoBusDb
                                .FirstOrDefaultAsync(a => a.IdUsuarioPiloto == piloto.Id && a.EsActual);

                            if (asignacionExistente == null)
                            {
                                var asignacionPiloto = new AsignacionPilotoBus
                                {
                                    IdUsuarioPiloto = piloto.Id,
                                    IdBus = 1,
                                    FechaAsignacion = DateTime.Now,
                                    EsActual = true,
                                    Activo = 1,
                                    FechaRegistro = DateTime.Now
                                };
                                _context.AsignacionesPilotoBusDb.Add(asignacionPiloto);
                                mensajes.Add("✅ Bus #1 asignado al Piloto");
                            }
                            else
                            {
                                mensajes.Add("ℹ️ Piloto ya tenía un bus asignado");
                            }
                        }
                        else
                        {
                            mensajes.Add("⚠️ No existe Bus #1 en la base de datos. Crear bus primero.");
                        }
                    }
                    else
                    {
                        mensajes.Add($"❌ Error al crear Piloto: {string.Join(", ", resultPiloto.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    mensajes.Add("ℹ️ Usuario Piloto ya existe");
                }

                // ========================================
                // 2. CREAR USUARIO MONITOR
                // ========================================
                var monitor = await _userManager.FindByEmailAsync("monitor1@transportesgenesis.com");
                if (monitor == null)
                {
                    monitor = new AppUser
                    {
                        UserName = "monitor1",
                        Email = "monitor1@transportesgenesis.com",
                        EmailConfirmed = true,
                        IsFirstLogin = false,
                        LastLoginDate = DateTime.Now
                    };

                    var resultMonitor = await _userManager.CreateAsync(monitor, "Monitor123!");
                    if (resultMonitor.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(monitor, "Monitor");
                        mensajes.Add("✅ Usuario Monitor creado exitosamente");

                        // Asignar mismo bus que el piloto (Bus #1)
                        var busExiste = await _context.BusesDb.AnyAsync(b => b.IdBus == 1);
                        if (busExiste)
                        {
                            // Verificar si ya tiene asignación
                            var asignacionExistente = await _context.AsignacionesPilotoBusDb
                                .FirstOrDefaultAsync(a => a.IdUsuarioPiloto == monitor.Id && a.EsActual);

                            if (asignacionExistente == null)
                            {
                                var asignacionMonitor = new AsignacionPilotoBus
                                {
                                    IdUsuarioPiloto = monitor.Id,
                                    IdBus = 1,
                                    FechaAsignacion = DateTime.Now,
                                    EsActual = true,
                                    Activo = 1,
                                    FechaRegistro = DateTime.Now
                                };
                                _context.AsignacionesPilotoBusDb.Add(asignacionMonitor);
                                mensajes.Add("✅ Bus #1 asignado al Monitor");
                            }
                            else
                            {
                                mensajes.Add("ℹ️ Monitor ya tenía un bus asignado");
                            }
                        }
                        else
                        {
                            mensajes.Add("⚠️ No existe Bus #1 en la base de datos. Crear bus primero.");
                        }
                    }
                    else
                    {
                        mensajes.Add($"❌ Error al crear Monitor: {string.Join(", ", resultMonitor.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    mensajes.Add("ℹ️ Usuario Monitor ya existe");
                }

                // Guardar cambios
                await _context.SaveChangesAsync();

                // ========================================
                // 3. RESUMEN FINAL
                // ========================================
                mensajes.Add("");
                mensajes.Add("========================================");
                mensajes.Add("📋 USUARIOS DE PRUEBA");
                mensajes.Add("========================================");
                mensajes.Add("");
                mensajes.Add("🔐 PILOTO:");
                mensajes.Add("   Email: piloto1@transportesgenesis.com");
                mensajes.Add("   Password: Piloto123!");
                mensajes.Add("   Rol: Piloto");
                mensajes.Add("   Bus Asignado: #1");
                mensajes.Add("");
                mensajes.Add("🔐 MONITOR:");
                mensajes.Add("   Email: monitor1@transportesgenesis.com");
                mensajes.Add("   Password: Monitor123!");
                mensajes.Add("   Rol: Monitor");
                mensajes.Add("   Bus Asignado: #1");
                mensajes.Add("");
                mensajes.Add("========================================");
                mensajes.Add("✅ Proceso completado exitosamente");
                mensajes.Add("========================================");
                mensajes.Add("");
                mensajes.Add("🔗 Siguiente paso: Cerrar sesión e intentar login con estos usuarios");

                return Content(string.Join("\n", mensajes), "text/plain");
            }
            catch (Exception ex)
            {
                return Content($"❌ ERROR: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "text/plain");
            }
        }
    }
}
