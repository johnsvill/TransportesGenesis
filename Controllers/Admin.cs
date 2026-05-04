using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models;
using TransportesGenesis.Models.DB.Usuarios;
using TransportesGenesis.ViewModels;

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
    
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GestionUsuarios()
    {
        var usuarios = _userManager.Users.ToList();

        var modelo = new List<UsuarioViewModel>();

        foreach (var usuario in usuarios)
        {
            var roles = await _userManager.GetRolesAsync(usuario);

            modelo.Add(new UsuarioViewModel
            {
                Email = usuario.Email,
                Roles = roles,
                UltimoLogin = usuario.LastLoginDate,
                Activo = usuario.LockoutEnd == null || usuario.LockoutEnd <= DateTimeOffset.Now,
                PrimerLoginPendiente = usuario.IsFirstLogin
            });
        }

        return View(modelo);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CrearUsuario(string Email, string Rol)
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Rol))
        {
            TempData["Error"] = "Debe ingresar correo y rol.";
            return RedirectToAction("Index"); 
        }

        var usuario = new AppUser
        {
            UserName = Email,
            Email = Email,
            IsFirstLogin = true, 
            LastLoginDate = null
        };

      
        var resultado = await _userManager.CreateAsync(usuario, "Temp123!");
        if (resultado.Succeeded)
        {
            await _userManager.AddToRoleAsync(usuario, Rol);
            TempData["Mensaje"] = "Usuario creado correctamente.";
        }
        else
        {
            TempData["Error"] = string.Join(", ", resultado.Errors.Select(e => e.Description));
        }

        return RedirectToAction("Index");
    }


    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<IActionResult> AprobarPago(int id)
    {
        var pago = await _context.PagosPadresDb.FindAsync(id);
        if (pago != null)
        {
            pago.EstadoAdmin = "Validado";
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("DashboardPagos");
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<IActionResult> RechazarPago(int id)
    {
        var pago = await _context.PagosPadresDb.FindAsync(id);
        if (pago != null)
        {
            pago.EstadoAdmin = "Rechazado";
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("DashboardPagos");
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult DashboardPagos(string usuarioId, string mes, int page = 1)
    {
        int pageSize = 10;
        var query = _context.PagosPadresDb.AsQueryable();

        if (!string.IsNullOrEmpty(usuarioId))
            query = query.Where(p => p.UsuarioId == usuarioId);

        if (!string.IsNullOrEmpty(mes))
            query = query.Where(p => p.Mes == mes);

        var totalRegistros = query.Count();
        var pagos = query
            .OrderByDescending(p => p.Fecha)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize);
        ViewBag.PaginaActual = page;
        
        var usuarios = _userManager.Users.ToList();
        var usuariosFiltrados = new List<string>();
        foreach (var u in usuarios)
        {
            var roles = _userManager.GetRolesAsync(u).Result;
            if (!roles.Contains("Administrador"))
                usuariosFiltrados.Add(u.Email);
        }
        ViewBag.Usuarios = usuariosFiltrados;

        ViewBag.Meses = _context.PagosPadresDb
            .Select(p => p.Mes)
            .Distinct()
            .ToList();
        
        ViewBag.UsuarioSeleccionado = usuarioId;
        ViewBag.MesSeleccionado = mes;

        return View(pagos);
    }


    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<IActionResult> DeshacerPago(int id)
    {
        var pago = await _context.PagosPadresDb.FindAsync(id);
        if (pago != null)
        {            
            if (pago.EstadoAdmin == "Validado" || pago.EstadoAdmin == "Rechazado")
            {
                pago.EstadoAdmin = "Pendiente";
                await _context.SaveChangesAsync();
            }
        }

        return RedirectToAction("DashboardPagos");
    }
}
