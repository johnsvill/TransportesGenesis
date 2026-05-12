using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Pagos()
        {
            var usuarioId = User.Identity.Name;
            var anioActual = DateTime.Now.Year;
            var mesActual = DateTime.Now.Month;
            var diaActual = DateTime.Now.Day;

            var mesesDisponibles = new[] { "Enero","Febrero","Marzo","Abril","Mayo","Junio",
                                                "Julio","Agosto","Septiembre","Octubre" };

            var pagosUsuario = _context.PagosPadresDb
                .Where(p => p.UsuarioId == usuarioId && p.Anio == anioActual)
                .ToList();

            var mesesPendientes = new List<string>();

            for (int i = 0; i < mesActual; i++)
            {
                var mesNombre = mesesDisponibles[i];
                var pago = pagosUsuario.FirstOrDefault(p => p.Mes == mesNombre);

                if (pago == null)
                {
                    if (i + 1 == mesActual && diaActual <= 5)
                        mesesPendientes.Add(mesNombre); // mes actual antes del día 5
                    else
                        mesesPendientes.Add(mesNombre + " (vencido)");
                }
                else
                {
                    if (pago.EstadoAdmin == "Pendiente")
                        mesesPendientes.Add(mesNombre + " (pendiente de validar)");
                    else if (pago.EstadoAdmin == "Rechazado")
                        mesesPendientes.Add(mesNombre + " (vencido)");
                    // Validado → no se agrega
                }
            }

            var montoAsignado = _context.MontoPadreDb
                .Where(m => m.UsuarioId == usuarioId && m.Activo == 1)
                .Select(m => m.MontoAsignado)
                .FirstOrDefault();

            var bancos = _context.BancosDb.Where(b => b.Activo == 1).ToList();
            var cuentas = _context.CuentasUsuarios.Where(c => c.UsuarioId == usuarioId).ToList();

            ViewBag.MesesPendientes = mesesPendientes;
            ViewBag.MontoMensual = montoAsignado;
            ViewBag.Bancos = bancos;
            ViewBag.Cuentas = cuentas;
            ViewData["StripeKey"] = _config["Stripe:PublishableKey"];
            ViewBag.EstaAlDia = !mesesPendientes.Any();

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarPago(string Mes)
        {
            var usuarioId = User.Identity.Name;
            var anioActual = DateTime.Now.Year;

            // 🔥 Guardar solo el nombre limpio del mes
            var mesLimpio = Mes.Split(' ')[0];

            var existePago = _context.PagosPadresDb.Any(p => p.UsuarioId == usuarioId && p.Mes == mesLimpio && p.Anio == anioActual);
            if (existePago)
            {
                TempData["Error"] = "Ya existe un pago registrado para este período.";
                return RedirectToAction("Pagos");
            }

            var montoAsignado = _context.MontoPadreDb
                .Where(m => m.UsuarioId == usuarioId && m.Activo == 1)
                .Select(m => m.MontoAsignado)
                .FirstOrDefault();

            var pago = new PagoPadre
            {
                UsuarioId = usuarioId,
                Monto = montoAsignado,
                Fecha = DateTime.Now,
                TipoPago = "Manual",
                ComprobanteUrl = null,
                Mes = mesLimpio, 
                Anio = anioActual,
                EstadoAdmin = "Pendiente",
                EstadoStripe = "Pendiente",
                FechaPago = DateTime.Now
            };

            _context.PagosPadresDb.Add(pago);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"Pago registrado para {mesLimpio} {anioActual}, pendiente de validación.";
            return RedirectToAction("Historial");
        }



        public IActionResult Historial()
        {
            var usuarioId = User.Identity.Name;
            var pagos = _context.PagosPadresDb
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.Fecha)
                .ToList();

            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            var service = new PaymentIntentService();

            foreach (var pago in pagos.Where(p => p.TipoPago == "Linea"))
            {
                try
                {
                    var intent = service.Get(pago.ComprobanteUrl);
                    pago.EstadoStripe = intent.Status;
                }
                catch { pago.EstadoStripe = "Error"; }
            }

            return View(pagos);
        }

        [HttpPost]
        public async Task<IActionResult> PagarEnLinea([FromBody] PagoRequest request)
        {
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            var usuarioId = User.Identity.Name;

            // 🔥 limpiar sufijo
            var mesSeleccionado = request.Mes.Split(' ')[0];
            var anioActual = DateTime.Now.Year;

            var existePago = _context.PagosPadresDb.Any(p =>
                p.UsuarioId == usuarioId &&
                p.Mes == mesSeleccionado &&
                p.Anio == anioActual);

            if (existePago)
            {
                return BadRequest(new { error = "Ya existe un pago registrado para este período." });
            }

            var montoAsignado = _context.MontoPadreDb
                .Where(m => m.UsuarioId == usuarioId && m.Activo == 1)
                .Select(m => m.MontoAsignado)
                .FirstOrDefault();

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(montoAsignado * 100),
                Currency = "gtq",
                Metadata = new Dictionary<string, string>
        {
            { "UsuarioId", usuarioId },
            { "Mes", mesSeleccionado },
            { "Anio", anioActual.ToString() }
        }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            var pago = new PagoPadre
            {
                UsuarioId = usuarioId,
                Monto = montoAsignado,
                Fecha = DateTime.Now,
                TipoPago = "Linea",
                ComprobanteUrl = intent.Id,
                Mes = mesSeleccionado,
                Anio = anioActual,
                EstadoAdmin = "Pendiente",
                EstadoStripe = "Pendiente"
            };

            _context.PagosPadresDb.Add(pago);
            await _context.SaveChangesAsync();

            return Json(new { clientSecret = intent.ClientSecret });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubirBoleta(IFormFile boleta, string mes, decimal monto)
        {
            if (boleta == null || boleta.Length == 0)
            {
                TempData["Error"] = "Debe subir un archivo.";
                return RedirectToAction("Pagos");
            }

            var extension = Path.GetExtension(boleta.FileName).ToLower();
            if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
            {
                TempData["Error"] = "Formato inválido. Solo se permiten archivos .jpg, .jpeg o .png.";
                return RedirectToAction("Pagos");
            }

            if (boleta.ContentType != "image/jpeg" && boleta.ContentType != "image/png")
            {
                TempData["Error"] = "El archivo no es una imagen válida.";
                return RedirectToAction("Pagos");
            }

            try
            {
                using var img = Image.Load(boleta.OpenReadStream());
            }
            catch
            {
                TempData["Error"] = "El archivo no contiene una imagen válida.";
                return RedirectToAction("Pagos");
            }

            var usuarioId = User.Identity.Name;
            var anioActual = DateTime.Now.Year;

          
            var mesLimpio = mes.Split(' ')[0];

            var existePago = _context.PagosPadresDb.Any(p => p.UsuarioId == usuarioId && p.Mes == mesLimpio && p.Anio == anioActual);
            if (existePago)
            {
                TempData["Error"] = "Ya existe un pago registrado para este período.";
                return RedirectToAction("Pagos");
            }

            string nombreArchivo = $"{Path.GetFileNameWithoutExtension(boleta.FileName)}_{DateTime.Now:ddMMyyyy_HHmmss}{extension}";
            string rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BoletasPago", nombreArchivo);

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await boleta.CopyToAsync(stream);
            }

            var pago = new PagoPadre
            {
                UsuarioId = usuarioId,
                Monto = monto,
                Fecha = DateTime.Now,              
                TipoPago = "Boleta",
                ComprobanteUrl = "/BoletasPago/" + nombreArchivo,
                Mes = mesLimpio, 
                Anio = anioActual,
                EstadoAdmin = "Pendiente",
                EstadoStripe = "Pendiente"
            };

            _context.PagosPadresDb.Add(pago);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"Pago registrado para {mesLimpio} {anioActual}.";
            return RedirectToAction("Historial");
        }


        [Authorize(Roles = "Administrador")]
        public IActionResult DashboardPagos()
        {
            var pagos = _context.PagosPadresDb.OrderByDescending(p => p.Fecha).ToList();
            return View(pagos);
        }
    }
}