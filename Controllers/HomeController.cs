using Microsoft.AspNetCore.Mvc;

namespace TransportesGenesis.Controllers
{
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
                    // Placeholder para piloto
                    return View("PilotoDashboard");
                }
                else if (User.IsInRole("Monitor"))
                {
                    // Placeholder para monitor
                    return View("MonitorDashboard");
                }
            }

            // Si no está logueado → Index público
            return View(); // aquí se carga Views/Home/Index.cshtml
        }
    }
}
