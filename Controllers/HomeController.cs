using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TransportesGenesis.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("PadreDeFamilia"))
                {
                    // Redirige al panel de pagos del padre de familia
                    return RedirectToAction("Index", "PagosPadresFamilia");
                }
                else if (User.IsInRole("Administrador"))
                {
                    // Placeholder para administrador
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("Piloto"))
                {
                    // Redirigir al dashboard de Piloto (Razor Page)
                    return RedirectToPage("/Piloto/MiRuta");
                }
                else if (User.IsInRole("Monitor"))
                {
                    // Redirigir al dashboard de Monitor (Razor Page)
                    return RedirectToPage("/Monitor/MiRuta");
                }
            }

            // Si no está logueado → Index público
            return View(); // aquí se carga Views/Home/Index.cshtml
        }
    }
}
