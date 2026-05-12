using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models;
using TransportesGenesis.Models.DB.Usuarios;
using TransportesGenesis.ViewModels;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using static System.Net.Mime.MediaTypeNames;

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

    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult> Calendario()
    {
        var hoy = DateTime.Now;
        
        ViewBag.SemanaActual = System.Globalization.ISOWeek.GetWeekOfYear(hoy);

        return await Task.Run(() => View());
    }

    [Authorize(Roles = "Administrador")]
    public async Task<JsonResult> ObtenerPagosCalendario(DateTime start, DateTime end, string usuario, string tipo)
    {
        var query = _context.PagosPadresDb
            .Where(p => p.Fecha >= start && p.Fecha <= end);

        if (!string.IsNullOrEmpty(usuario))
            query = query.Where(p => p.UsuarioId.Contains(usuario));

        if (!string.IsNullOrEmpty(tipo))
            query = query.Where(p => p.TipoPago == tipo);

        var listaPagos = await query.ToListAsync();

        var eventosCalendario = listaPagos.Select(pago => new {
            title = $"Usuario: {pago.UsuarioId}",
            start = pago.Fecha.ToString("yyyy-MM-dd"),
            color = (pago.TipoPago == "Boleta") ? "#007bff" : "#28a745",
            usuario = pago.UsuarioId,
            monto = pago.Monto,
            tipo = pago.TipoPago,
            comprobante = pago.ComprobanteUrl,
            estado = pago.EstadoAdmin
        });

        return Json(eventosCalendario);
    }

    [HttpGet]
    public async Task<IActionResult> ValidarPago(int id)
    {
        var pago = await _context.PagosPadresDb.FindAsync(id);
        if (pago == null) return NotFound();

        pago.EstadoAdmin = "Validado";
        _context.Update(pago);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Pago validado correctamente.";
        return RedirectToAction("Calendario");
    }
    
    [HttpGet]
    public IActionResult ExportarPagosExcel()
    {
        var pagos = _context.PagosPadresDb.ToList();

        using (var workbook = new XLWorkbook())
        {
            var ws = workbook.Worksheets.Add("Pagos");
            
            ws.Cell(1, 1).Value = "Usuario";
            ws.Cell(1, 2).Value = "Monto";
            ws.Cell(1, 3).Value = "Tipo";
            ws.Cell(1, 4).Value = "Fecha";
            ws.Cell(1, 5).Value = "Estado";

            var headerRange = ws.Range(1, 1, 1, 5);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int fila = 2;
            foreach (var pago in pagos)
            {
                ws.Cell(fila, 1).Value = pago.UsuarioId;
                ws.Cell(fila, 2).Value = pago.Monto;
                ws.Cell(fila, 3).Value = pago.TipoPago;
                ws.Cell(fila, 4).Value = pago.Fecha.ToString("yyyy-MM-dd");
                ws.Cell(fila, 5).Value = pago.EstadoAdmin;
                fila++;
            }
            
            ws.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Pagos.xlsx");
            }
        }
    }

    [HttpGet]
    public IActionResult ExportarPagosPdf()
    {
        var pagos = _context.PagosPadresDb.ToList();

        using (var ms = new MemoryStream())
        {
            var doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();
           
            var fontTitulo = new iTextSharp.text.Font(
                iTextSharp.text.Font.HELVETICA,
                16,                            
                iTextSharp.text.Font.BOLD    
            );
            
            var titulo = new Paragraph("Reporte de Pagos", fontTitulo)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(titulo);
            doc.Add(new Paragraph(" "));
            
            PdfPTable tabla = new PdfPTable(5);
            tabla.WidthPercentage = 100;
            tabla.SetWidths(new float[] { 2, 1, 1, 1.5f, 1 });
            
            string[] headers = { "Usuario", "Monto", "Tipo", "Fecha", "Estado" };
            foreach (var h in headers)
            {
                var cell = new PdfPCell(new Phrase(h))
                {
                    BackgroundColor = new BaseColor(211, 211, 211),
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                tabla.AddCell(cell);
            }
            
            foreach (var pago in pagos)
            {
                tabla.AddCell(pago.UsuarioId);
                tabla.AddCell(new PdfPCell(new Phrase("Q " + pago.Monto))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT
                });
                tabla.AddCell(pago.TipoPago);
                tabla.AddCell(pago.Fecha.ToString("yyyy-MM-dd"));
                tabla.AddCell(pago.EstadoAdmin ?? "Pendiente");
            }

            doc.Add(tabla);
            doc.Close();

            return File(ms.ToArray(), "application/pdf", "Pagos.pdf");
        }
    }
}
