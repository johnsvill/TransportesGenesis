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
    public class ReporteAsistenciasModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReporteAsistenciasModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<AsistenciaAlumno> Asistencias { get; set; } = new List<AsistenciaAlumno>();
        public List<Alumnos> AlumnosDisponibles { get; set; } = new List<Alumnos>();
        public List<Ruta> RutasDisponibles { get; set; } = new List<Ruta>();

        [BindProperty(SupportsGet = true)]
        public int? FiltroAlumno { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FiltroRuta { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaDesde { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaHasta { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Mes { get; set; } = DateTime.Now.Month;

        [BindProperty(SupportsGet = true)]
        public int Año { get; set; } = DateTime.Now.Year;

        // Estadísticas
        public int TotalAsistenciasMañana { get; set; }
        public int TotalAsistenciasTarde { get; set; }
        public int TotalAusenciasMañana { get; set; }
        public int TotalAusenciasTarde { get; set; }
        public decimal PorcentajeAsistenciaMañana { get; set; }
        public decimal PorcentajeAsistenciaTarde { get; set; }
        public Dictionary<string, int> AsistenciasPorAlumno { get; set; } = new Dictionary<string, int>();

        public async Task OnGetAsync()
        {
            await CargarDatos();
        }

        public async Task<IActionResult> OnGetExportarExcelAsync()
        {
            await CargarDatos();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Reporte de Asistencias");

            // Encabezados
            worksheet.Cell(1, 1).Value = "Alumno";
            worksheet.Cell(1, 2).Value = "Fecha";
            worksheet.Cell(1, 3).Value = "Asiste Mañana";
            worksheet.Cell(1, 4).Value = "Asiste Tarde";
            worksheet.Cell(1, 5).Value = "Bus Temporal Mañana";
            worksheet.Cell(1, 6).Value = "Bus Temporal Tarde";
            worksheet.Cell(1, 7).Value = "Fecha Confirmación";

            // Formato de encabezados
            var headerRange = worksheet.Range(1, 1, 1, 7);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#28a745");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int fila = 2;
            foreach (var asistencia in Asistencias)
            {
                worksheet.Cell(fila, 1).Value = $"{asistencia.Alumno.Nombre} {asistencia.Alumno.Apellido}";
                worksheet.Cell(fila, 2).Value = asistencia.Fecha.ToString("dd/MM/yyyy");
                worksheet.Cell(fila, 3).Value = asistencia.AsisteMañana ? "Sí" : "No";
                worksheet.Cell(fila, 4).Value = asistencia.AsisteTarde ? "Sí" : "No";
                worksheet.Cell(fila, 5).Value = asistencia.BusTemporalMañana?.Placa ?? "-";
                worksheet.Cell(fila, 6).Value = asistencia.BusTemporalTarde?.Placa ?? "-";
                worksheet.Cell(fila, 7).Value = asistencia.FechaConfirmacion?.ToString("dd/MM/yyyy HH:mm") ?? "No confirmada";

                // Colores según asistencia
                if (!asistencia.AsisteMañana)
                {
                    worksheet.Cell(fila, 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#dc3545");
                    worksheet.Cell(fila, 3).Style.Font.FontColor = XLColor.White;
                }
                if (!asistencia.AsisteTarde)
                {
                    worksheet.Cell(fila, 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#dc3545");
                    worksheet.Cell(fila, 4).Style.Font.FontColor = XLColor.White;
                }

                fila++;
            }

            // Agregar estadísticas al final
            fila += 2;
            worksheet.Cell(fila, 1).Value = "ESTADÍSTICAS";
            worksheet.Cell(fila, 1).Style.Font.Bold = true;
            worksheet.Cell(fila, 1).Style.Font.FontSize = 14;
            fila++;

            worksheet.Cell(fila, 1).Value = "Total Asistencias Mañana:";
            worksheet.Cell(fila, 2).Value = TotalAsistenciasMañana;
            fila++;

            worksheet.Cell(fila, 1).Value = "Total Asistencias Tarde:";
            worksheet.Cell(fila, 2).Value = TotalAsistenciasTarde;
            fila++;

            worksheet.Cell(fila, 1).Value = "Total Ausencias Mañana:";
            worksheet.Cell(fila, 2).Value = TotalAusenciasMañana;
            fila++;

            worksheet.Cell(fila, 1).Value = "Total Ausencias Tarde:";
            worksheet.Cell(fila, 2).Value = TotalAusenciasTarde;
            fila++;

            worksheet.Cell(fila, 1).Value = "% Asistencia Mañana:";
            worksheet.Cell(fila, 2).Value = $"{PorcentajeAsistenciaMañana:F2}%";
            fila++;

            worksheet.Cell(fila, 1).Value = "% Asistencia Tarde:";
            worksheet.Cell(fila, 2).Value = $"{PorcentajeAsistenciaTarde:F2}%";

            // Ajustar anchos
            worksheet.Column(1).Width = 30;
            worksheet.Column(2).Width = 15;
            worksheet.Column(3).Width = 15;
            worksheet.Column(4).Width = 15;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 20;
            worksheet.Column(7).Width = 25;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var nombreArchivo = $"ReporteAsistencias_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
        }

        private async Task CargarDatos()
        {
            // Cargar listas para filtros
            AlumnosDisponibles = await _context.AlumnosDb
                .OrderBy(a => a.Nombre)
                .ToListAsync();

            RutasDisponibles = await _context.RutasDb
                .Include(r => r.Bus)
                .OrderBy(r => r.Nombre)
                .ToListAsync();

            // Consulta base
            var query = _context.AsistenciasAlumnoDb
                .Include(a => a.Alumno)
                .ThenInclude(al => al.BusAsignado)
                .Include(a => a.BusTemporalMañana)
                .Include(a => a.BusTemporalTarde)
                .AsQueryable();

            // Aplicar filtros
            if (FiltroAlumno.HasValue && FiltroAlumno.Value > 0)
            {
                query = query.Where(a => a.IdAlumno == FiltroAlumno.Value);
            }

            // Si hay filtro de fecha específico
            if (FechaDesde.HasValue)
            {
                query = query.Where(a => a.Fecha >= FechaDesde.Value);
            }

            if (FechaHasta.HasValue)
            {
                query = query.Where(a => a.Fecha <= FechaHasta.Value);
            }

            // Si no hay filtro de fechas, usar mes y año
            if (!FechaDesde.HasValue && !FechaHasta.HasValue)
            {
                var primerDia = new DateTime(Año, Mes, 1);
                var ultimoDia = primerDia.AddMonths(1).AddDays(-1);
                query = query.Where(a => a.Fecha >= primerDia && a.Fecha <= ultimoDia);
            }

            Asistencias = await query
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();

            // Calcular estadísticas
            TotalAsistenciasMañana = Asistencias.Count(a => a.AsisteMañana);
            TotalAsistenciasTarde = Asistencias.Count(a => a.AsisteTarde);
            TotalAusenciasMañana = Asistencias.Count(a => !a.AsisteMañana);
            TotalAusenciasTarde = Asistencias.Count(a => !a.AsisteTarde);

            var totalRegistros = Asistencias.Count();
            if (totalRegistros > 0)
            {
                PorcentajeAsistenciaMañana = (decimal)TotalAsistenciasMañana / totalRegistros * 100;
                PorcentajeAsistenciaTarde = (decimal)TotalAsistenciasTarde / totalRegistros * 100;
            }

            // Asistencias por alumno
            AsistenciasPorAlumno = Asistencias
                .GroupBy(a => $"{a.Alumno.Nombre} {a.Alumno.Apellido}")
                .ToDictionary(g => g.Key, g => g.Count(a => a.AsisteMañana || a.AsisteTarde));
        }
    }
}
