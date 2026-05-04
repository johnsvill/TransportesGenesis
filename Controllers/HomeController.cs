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
                    return RedirectToAction("Index", "PagosPadresFamilia");
                }
                else if (User.IsInRole("Administrador"))
                {                    
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("Piloto"))
                {                    
                    return View("PilotoDashboard");
                }
                else if (User.IsInRole("Monitor"))
                {                    
                    return View("MonitorDashboard");
                }
            }         
            return View(); 
        }
    }
}
