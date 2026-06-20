using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class MontosPadresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MontosPadresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {            
            var montos = _context.MontosPadresView
                .FromSqlRaw("EXEC sp_GetMontosPadres")
                .ToList();

            return View(montos);
        }

        public IActionResult Create()
        {
            var padres = _context.Users
                .Where(u => !_context.UserRoles
                    .Any(r => r.UserId == u.Id && r.RoleId == _context.Roles.FirstOrDefault(ro => ro.Name == "Administrador").Id))
                .Select(u => new { u.UserName, u.Email })
                .ToList();

            ViewBag.Padres = padres;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MontoPadre montoPadre)
        {
            if (ModelState.IsValid)
            {
                montoPadre.Activo = 1;
                _context.Add(montoPadre);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "✅ El monto fue asignado correctamente.";
                return RedirectToAction("Index", "MontosPadres");
            }
            return View(montoPadre);
        }

        public IActionResult Edit(int id)
        {
            var monto = _context.MontoPadreDb.Find(id);
            if (monto == null) return NotFound();

            var usuario = _context.Users.FirstOrDefault(u => u.UserName == monto.UsuarioId);
            ViewBag.Correo = usuario?.Email;

            return View(monto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MontoPadre montoPadre)
        {
            if (id != montoPadre.IdMontoPadre) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(montoPadre);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "✅ El monto fue actualizado correctamente.";
                return RedirectToAction("Index", "MontosPadres");
            }
            return View(montoPadre);
        }

        public IActionResult Delete(int id)
        {
            var monto = _context.MontoPadreDb.Find(id);
            if (monto == null) return NotFound();

            var usuario = _context.Users.FirstOrDefault(u => u.UserName == monto.UsuarioId);
            ViewBag.Correo = usuario?.Email;

            return View(monto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdMontoPadre)
        {
            var monto = _context.MontoPadreDb.Find(IdMontoPadre);
            if (monto != null)
            {
                monto.Activo = 0;
                _context.Update(monto);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "✅ El monto fue dado de baja correctamente.";
            }
            return RedirectToAction("Index", "MontosPadres");
        }
    }
}
