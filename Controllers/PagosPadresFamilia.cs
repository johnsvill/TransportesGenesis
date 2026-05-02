using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Models.DTO;

namespace TransportesGenesis.Controllers
{
    [Authorize(Roles = "PadreDeFamilia")]
    public class PagosPadresFamiliaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public PagosPadresFamiliaController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Index con botones grandes
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Pagos()
        {
            var mesesDisponibles = new List<string>
            {
                "Enero","Febrero","Marzo","Abril","Mayo","Junio",
                "Julio","Agosto","Septiembre","Octubre"
            };

            var anioActual = DateTime.Now.Year;
            var mesActual = DateTime.Now.Month;

            var pagosRealizados = _context.PagosPadres
                .Where(p => p.UsuarioId == User.Identity.Name && p.Anio == anioActual)
                .Select(p => p.Mes)
                .ToList();

            var mesesPendientes = mesesDisponibles
                .Take(mesActual)
                .Where(m => !pagosRealizados.Contains(m))
                .ToList();
            
            bool mesActualPagado = pagosRealizados.Contains(mesesDisponibles[mesActual - 1]);

            var mesesHabilitados = mesesPendientes.Any()
                ? mesesPendientes
                : (mesActualPagado ? new List<string>() : new List<string> { mesesDisponibles[mesActual - 1] });

            ViewBag.MesesDisponibles = mesesHabilitados;
            ViewBag.MesesPendientes = mesesPendientes;
            ViewBag.MesActualPagado = mesActualPagado;

            ViewData["StripeKey"] = _config["Stripe:PublishableKey"];
            return View();
        }

        public IActionResult Historial()
        {
            var usuarioId = User.Identity.Name;
            var pagos = _context.PagosPadres
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.Fecha)
                .ToList();

            return View(pagos);
        }

        [HttpPost]
        public async Task<IActionResult> PagarEnLinea([FromBody] PagoRequest request)
        {
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(request.Monto * 100),
                Currency = "gtq",
                Metadata = new Dictionary<string, string>
                {
                    { "UsuarioId", User.Identity.Name },
                    { "Mes", request.Mes },
                    { "Anio", DateTime.Now.Year.ToString() }
                }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);
        
            var pago = new PagoPadre
            {
                UsuarioId = User.Identity.Name,
                Monto = request.Monto,
                Fecha = DateTime.Now,
                TipoPago = "Linea",
                ComprobanteUrl = intent.Id,
                Mes = request.Mes,
                Anio = DateTime.Now.Year
            };

            _context.PagosPadres.Add(pago);
            await _context.SaveChangesAsync();
           
            return Json(new { clientSecret = intent.ClientSecret });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]      
        public async Task<IActionResult> SubirBoleta(IFormFile boleta, decimal monto, string mes)
        {
            if (boleta != null && boleta.Length > 0)
            {
                string extension = Path.GetExtension(boleta.FileName);
                string baseName = Path.GetFileNameWithoutExtension(boleta.FileName);
                string timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                string nombreArchivo = $"{baseName}_{timestamp}{extension}";
                string rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BoletasPago", nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await boleta.CopyToAsync(stream);
                }

                string rutaRelativa = "/BoletasPago/" + nombreArchivo;

                var pago = new PagoPadre
                {
                    UsuarioId = User.Identity.Name,
                    Monto = monto,
                    Fecha = DateTime.Now,
                    TipoPago = "Boleta",
                    ComprobanteUrl = rutaRelativa,
                    Mes = mes,              
                    Anio = DateTime.Now.Year  
                };

                _context.PagosPadres.Add(pago);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Historial");
        }
    }
}
