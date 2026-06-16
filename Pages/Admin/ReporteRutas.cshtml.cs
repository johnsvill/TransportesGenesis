using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using ClosedXML.Excel;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class ReporteRutasModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReporteRutasModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Ruta> Rutas { get; set; } = new List<Ruta>();

        public async Task OnGetAsync()
        {
            Rutas = await _context.RutasDb
                .Include(r => r.Bus)
                .Include(r => r.ParadasLink)
                .OrderByDescending(r => r.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IActionResult> OnGetExportarExcelAsync()
        {
            Rutas = await _context.RutasDb
                .Include(r => r.Bus)
                .Include(r => r.ParadasLink)
                .OrderByDescending(r => r.FechaRegistro)
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Reporte de Rutas");

            // Encabezados
            worksheet.Cell(1, 1).Value = "ID Ruta";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Descripción";
            worksheet.Cell(1, 4).Value = "Bus (Placa)";
            worksheet.Cell(1, 5).Value = "Tipo Ruta";
            worksheet.Cell(1, 6).Value = "Hora Inicio";
            worksheet.Cell(1, 7).Value = "Nº Paradas";
            worksheet.Cell(1, 8).Value = "Estado";
            worksheet.Cell(1, 9).Value = "Fecha Registro";

            // Formato de encabezados
            var headerRange = worksheet.Range(1, 1, 1, 9);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#0d6efd");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int fila = 2;
            foreach (var ruta in Rutas)
            {
                worksheet.Cell(fila, 1).Value = ruta.IdRuta;
                worksheet.Cell(fila, 2).Value = ruta.Nombre;
                worksheet.Cell(fila, 3).Value = ruta.Descripcion ?? "-";
                worksheet.Cell(fila, 4).Value = ruta.Bus?.Placa ?? "Sin bus";
                worksheet.Cell(fila, 5).Value = ruta.TipoRuta;
                worksheet.Cell(fila, 6).Value = ruta.HoraInicio.ToString(@"hh\:mm");
                worksheet.Cell(fila, 7).Value = ruta.ParadasLink.Count;
                worksheet.Cell(fila, 8).Value = ruta.EsActiva ? "Activa" : "Inactiva";
                worksheet.Cell(fila, 9).Value = ruta.FechaRegistro.ToString("dd/MM/yyyy");

                // Colores según tipo de ruta
                if (ruta.TipoRuta == "Mañana")
                {
                    worksheet.Cell(fila, 5).Style.Fill.BackgroundColor = XLColor.FromHtml("#ffc107");
                }
                else if (ruta.TipoRuta == "Tarde")
                {
                    worksheet.Cell(fila, 5).Style.Fill.BackgroundColor = XLColor.FromHtml("#0d6efd");
                    worksheet.Cell(fila, 5).Style.Font.FontColor = XLColor.White;
                }

                // Color según estado
                if (ruta.EsActiva)
                {
                    worksheet.Cell(fila, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#28a745");
                    worksheet.Cell(fila, 8).Style.Font.FontColor = XLColor.White;
                }
                else
                {
                    worksheet.Cell(fila, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#6c757d");
                    worksheet.Cell(fila, 8).Style.Font.FontColor = XLColor.White;
                }

                fila++;
            }

            // Agregar estadísticas
            fila += 2;
            worksheet.Cell(fila, 1).Value = "ESTADÍSTICAS";
            worksheet.Cell(fila, 1).Style.Font.Bold = true;
            worksheet.Cell(fila, 1).Style.Font.FontSize = 14;
            fila++;

            worksheet.Cell(fila, 1).Value = "Total de Rutas:";
            worksheet.Cell(fila, 2).Value = Rutas.Count;
            fila++;

            worksheet.Cell(fila, 1).Value = "Rutas Activas:";
            worksheet.Cell(fila, 2).Value = Rutas.Count(r => r.EsActiva);
            fila++;

            worksheet.Cell(fila, 1).Value = "Rutas Inactivas:";
            worksheet.Cell(fila, 2).Value = Rutas.Count(r => !r.EsActiva);
            fila++;

            worksheet.Cell(fila, 1).Value = "Rutas Mañana:";
            worksheet.Cell(fila, 2).Value = Rutas.Count(r => r.TipoRuta == "Mañana");
            fila++;

            worksheet.Cell(fila, 1).Value = "Rutas Tarde:";
            worksheet.Cell(fila, 2).Value = Rutas.Count(r => r.TipoRuta == "Tarde");

            // Ajustar anchos
            worksheet.Column(1).Width = 10;
            worksheet.Column(2).Width = 30;
            worksheet.Column(3).Width = 40;
            worksheet.Column(4).Width = 15;
            worksheet.Column(5).Width = 15;
            worksheet.Column(6).Width = 12;
            worksheet.Column(7).Width = 12;
            worksheet.Column(8).Width = 12;
            worksheet.Column(9).Width = 15;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var nombreArchivo = $"ReporteRutas_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
        }

        public async Task<IActionResult> OnPostGenerarDatosEjemploAsync()
        {
            try
            {
                // Verificar si ya hay rutas
                var rutasExistentes = await _context.RutasDb.AnyAsync();
                if (rutasExistentes)
                {
                    TempData["Mensaje"] = "Ya existen rutas en el sistema.";
                    TempData["TipoMensaje"] = "info";
                    return RedirectToPage();
                }

                // Verificar o crear buses
                var buses = await _context.BusesDb.ToListAsync();
                if (!buses.Any())
                {
                    // Crear buses de ejemplo
                    var busesEjemplo = new List<Bus>
                    {
                        new Bus { Placa = "ABC-123", Modelo = "Mercedes Sprinter", Capacidad = 20, Estado = true, Activo = 1 },
                        new Bus { Placa = "XYZ-789", Modelo = "Toyota Coaster", Capacidad = 25, Estado = true, Activo = 1 },
                        new Bus { Placa = "DEF-456", Modelo = "Ford Transit", Capacidad = 15, Estado = true, Activo = 1 }
                    };
                    _context.BusesDb.AddRange(busesEjemplo);
                    await _context.SaveChangesAsync();
                    buses = await _context.BusesDb.ToListAsync();
                }

                // Crear rutas de ejemplo
                var rutasEjemplo = new List<Ruta>
                {
                    new Ruta
                    {
                        Nombre = "Ruta Norte - Mañana",
                        Descripcion = "Recorrido por zona norte de la ciudad",
                        IdBus = buses[0].IdBus,
                        TipoRuta = "Mañana",
                        HoraInicio = new TimeSpan(6, 30, 0),
                        EsActiva = true,
                        Activo = 1,
                        FechaRegistro = DateTime.Now
                    },
                    new Ruta
                    {
                        Nombre = "Ruta Sur - Tarde",
                        Descripcion = "Recorrido por zona sur",
                        IdBus = buses.Count > 1 ? buses[1].IdBus : buses[0].IdBus,
                        TipoRuta = "Tarde",
                        HoraInicio = new TimeSpan(13, 0, 0),
                        EsActiva = true,
                        Activo = 1,
                        FechaRegistro = DateTime.Now
                    },
                    new Ruta
                    {
                        Nombre = "Ruta Centro - Mañana",
                        Descripcion = "Recorrido por el centro de la ciudad",
                        IdBus = buses.Count > 2 ? buses[2].IdBus : buses[0].IdBus,
                        TipoRuta = "Mañana",
                        HoraInicio = new TimeSpan(7, 0, 0),
                        EsActiva = true,
                        Activo = 1,
                        FechaRegistro = DateTime.Now.AddDays(-1)
                    },
                    new Ruta
                    {
                        Nombre = "Ruta Este - Tarde",
                        Descripcion = "Recorrido zona este (inactiva para mantenimiento)",
                        IdBus = buses[0].IdBus,
                        TipoRuta = "Tarde",
                        HoraInicio = new TimeSpan(14, 30, 0),
                        EsActiva = false,
                        Activo = 0,
                        FechaRegistro = DateTime.Now.AddDays(-7)
                    }
                };

                _context.RutasDb.AddRange(rutasEjemplo);
                await _context.SaveChangesAsync();

                var busesCreados = await _context.BusesDb.CountAsync() - buses.Count;
                TempData["Mensaje"] = $"Se crearon {rutasEjemplo.Count} rutas de ejemplo" + (busesCreados > 0 ? $" y {busesCreados} buses" : " usando buses existentes") + ".";
                TempData["TipoMensaje"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al generar datos: {ex.Message}";
                TempData["TipoMensaje"] = "error";
            }

            return RedirectToPage();
        }
    }
}

