using Microsoft.AspNetCore.Mvc;

namespace TransportesGenesis.Controllers
{
    public class TestController : Controller
    {
        [Route("Test")]
        [Route("Test/Rutas")]
        [Route("Test/RutasAPI")]
        public IActionResult RutasAPI()
        {
            return View();
        }
    }
}
