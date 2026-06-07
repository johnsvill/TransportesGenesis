using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TransportesGenesis.DTOs.Notificaciones;
using TransportesGenesis.Services.Interfaces;
using ClosedXML.Excel;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class AlertasHistorialModel : PageModel
    {
        private readonly IAlertaService _alertaService;
        private readonly IBusService _busService;

        public AlertasHistorialModel(IAlertaService alertaService, IBusService busService)
        {
            _alertaService = alertaService;
            _busService = busService;
        }

        public IEnumerable<AlertaProximidadDto> Alertas { get; set; } = new List<AlertaProximidadDto>();
        public List<SelectListItem> BusesDisponibles { get; set; } = new List<SelectListItem>();

        [BindProperty(SupportsGet = true)]
        public string? FiltroTipo { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FiltroEstado { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FiltroBus { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FiltroAlumno { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FiltroFechaDesde { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FiltroFechaHasta { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? OrdenarPor { get; set; } = "FechaHora";

        [BindProperty(SupportsGet = true)]
        public bool OrdenDescendente { get; set; } = true;

        public string? Mensaje { get; set; }
        public string? TipoMensaje { get; set; }

        // Contadores para estadísticas
        public int TotalAlertas { get; set; }
        public int AlertasActivas { get; set; }
        public int AlertasResueltas { get; set; }
        public int AlertasProximidad { get; set; }
        public int AlertasRetraso { get; set; }

        public async Task OnGetAsync()
        {
            await CargarDatosAsync();
        }

        public async Task<IActionResult> OnPostMarcarResueltaAsync(int idAlerta)
        {
            try
            {
                var resultado = await _alertaService.MarcarAlertaComoResueltaAsync(idAlerta);

                if (resultado)
                {
                    TempData["Mensaje"] = "Alerta marcada como resuelta correctamente.";
                    TempData["TipoMensaje"] = "success";
                }
                else
                {
                    TempData["Mensaje"] = "No se pudo resolver la alerta. Puede que ya esté resuelta.";
                    TempData["TipoMensaje"] = "warning";
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al resolver la alerta: {ex.Message}";
                TempData["TipoMensaje"] = "error";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLimpiarAlertasAntiguasAsync()
        {
            try
            {
                var eliminadas = await _alertaService.LimpiarAlertasAntiguasAsync(30);

                TempData["Mensaje"] = $"Se eliminaron {eliminadas} alertas antiguas correctamente.";
                TempData["TipoMensaje"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al limpiar alertas: {ex.Message}";
                TempData["TipoMensaje"] = "error";
            }

            return RedirectToPage();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                // Cargar buses para el dropdown
                var buses = await _busService.GetBusesActivosAsync();
                BusesDisponibles = buses.Select(b => new SelectListItem
                {
                    Value = b.IdBus.ToString(),
                    Text = $"Bus {b.Placa} - {b.Modelo}"
                }).ToList();

                // Obtener todas las alertas
                var todasLasAlertas = new List<AlertaProximidadDto>();

                // Cargar alertas según filtros
                if (FiltroBus.HasValue)
                {
                    var alertasBus = await _alertaService.GetHistorialAlertasPorBusAsync(FiltroBus.Value, true);
                    todasLasAlertas.AddRange(alertasBus);
                }
                else if (FiltroAlumno.HasValue)
                {
                    var alertasAlumno = await _alertaService.GetHistorialAlertasPorAlumnoAsync(FiltroAlumno.Value, true);
                    todasLasAlertas.AddRange(alertasAlumno);
                }
                else
                {
                    // Cargar por tipo si está especificado
                    if (!string.IsNullOrEmpty(FiltroTipo))
                    {
                        var alertasTipo = await _alertaService.GetAlertasActivasPorTipoAsync(FiltroTipo);
                        todasLasAlertas.AddRange(alertasTipo);

                        // También cargar las resueltas del mismo tipo
                        // Para esto necesitaríamos un método en el servicio, por ahora simulamos
                    }
                    else
                    {
                        // Cargar alertas de proximidad y retraso
                        var proximidad = await _alertaService.GetAlertasActivasPorTipoAsync("proximidad");
                        var retraso = await _alertaService.GetAlertasActivasPorTipoAsync("retraso");
                        todasLasAlertas.AddRange(proximidad);
                        todasLasAlertas.AddRange(retraso);
                    }
                }

                // Aplicar filtros adicionales
                if (!string.IsNullOrEmpty(FiltroEstado))
                {
                    todasLasAlertas = todasLasAlertas.Where(a => a.Estado == FiltroEstado).ToList();
                }

                if (FiltroFechaDesde.HasValue)
                {
                    todasLasAlertas = todasLasAlertas.Where(a => a.FechaHora >= FiltroFechaDesde.Value).ToList();
                }

                if (FiltroFechaHasta.HasValue)
                {
                    todasLasAlertas = todasLasAlertas.Where(a => a.FechaHora <= FiltroFechaHasta.Value).ToList();
                }

                // Aplicar ordenamiento
                todasLasAlertas = AplicarOrdenamiento(todasLasAlertas).ToList();

                Alertas = todasLasAlertas;

                // Calcular estadísticas
                CalcularEstadisticas(todasLasAlertas);

                // Configurar mensajes de TempData
                if (TempData["Mensaje"] != null)
                {
                    Mensaje = TempData["Mensaje"].ToString();
                    TipoMensaje = TempData["TipoMensaje"]?.ToString();
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error cargando datos: {ex.Message}";
                TipoMensaje = "error";
            }
        }

        private IEnumerable<AlertaProximidadDto> AplicarOrdenamiento(IEnumerable<AlertaProximidadDto> alertas)
        {
            return OrdenarPor switch
            {
                "TipoAlerta" => OrdenDescendente ? alertas.OrderByDescending(a => a.TipoAlerta) : alertas.OrderBy(a => a.TipoAlerta),
                "Estado" => OrdenDescendente ? alertas.OrderByDescending(a => a.Estado) : alertas.OrderBy(a => a.Estado),
                "IdBus" => OrdenDescendente ? alertas.OrderByDescending(a => a.IdBus) : alertas.OrderBy(a => a.IdBus),
                "NombreBus" => OrdenDescendente ? alertas.OrderByDescending(a => a.NombreBus) : alertas.OrderBy(a => a.NombreBus),
                "NombreAlumno" => OrdenDescendente ? alertas.OrderByDescending(a => a.NombreAlumno) : alertas.OrderBy(a => a.NombreAlumno),
                _ => OrdenDescendente ? alertas.OrderByDescending(a => a.FechaHora) : alertas.OrderBy(a => a.FechaHora)
            };
        }

        private void CalcularEstadisticas(IEnumerable<AlertaProximidadDto> alertas)
        {
            TotalAlertas = alertas.Count();
            AlertasActivas = alertas.Count(a => a.Estado == "activo");
            AlertasResueltas = alertas.Count(a => a.Estado == "resuelto");
            AlertasProximidad = alertas.Count(a => a.TipoAlerta == "proximidad");
            AlertasRetraso = alertas.Count(a => a.TipoAlerta == "retraso");
        }

        public async Task<IActionResult> OnGetExportarExcelAsync()
        {
            await CargarDatosAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Historial de Alertas");

            // Encabezados
            worksheet.Cell(1, 1).Value = "ID Alerta";
            worksheet.Cell(1, 2).Value = "Fecha y Hora";
            worksheet.Cell(1, 3).Value = "Tipo de Alerta";
            worksheet.Cell(1, 4).Value = "Estado";
            worksheet.Cell(1, 5).Value = "Bus (Placa)";
            worksheet.Cell(1, 6).Value = "Alumno";
            worksheet.Cell(1, 7).Value = "Mensaje";
            worksheet.Cell(1, 8).Value = "Paradas Restantes";

            // Formato de encabezados
            var headerRange = worksheet.Range(1, 1, 1, 8);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#dc3545");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int fila = 2;
            foreach (var alerta in Alertas)
            {
                worksheet.Cell(fila, 1).Value = alerta.Id;
                worksheet.Cell(fila, 2).Value = alerta.FechaHora.ToString("dd/MM/yyyy HH:mm:ss");
                worksheet.Cell(fila, 3).Value = alerta.TipoAlerta;
                worksheet.Cell(fila, 4).Value = alerta.Estado;
                worksheet.Cell(fila, 5).Value = alerta.NombreBus ?? "-";
                worksheet.Cell(fila, 6).Value = alerta.NombreAlumno ?? "-";
                worksheet.Cell(fila, 7).Value = alerta.Mensaje ?? "-";
                worksheet.Cell(fila, 8).Value = alerta.ParadasRestantes?.ToString() ?? "-";

                // Colores según tipo de alerta
                if (alerta.TipoAlerta.ToLower() == "proximidad")
                {
                    worksheet.Cell(fila, 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#0dcaf0");
                }
                else if (alerta.TipoAlerta.ToLower() == "retraso")
                {
                    worksheet.Cell(fila, 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#ffc107");
                }

                // Color según estado
                if (alerta.Estado.ToLower() == "activo")
                {
                    worksheet.Cell(fila, 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#dc3545");
                    worksheet.Cell(fila, 4).Style.Font.FontColor = XLColor.White;
                }
                else if (alerta.Estado.ToLower() == "resuelto")
                {
                    worksheet.Cell(fila, 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#28a745");
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

            worksheet.Cell(fila, 1).Value = "Total de Alertas:";
            worksheet.Cell(fila, 2).Value = TotalAlertas;
            fila++;

            worksheet.Cell(fila, 1).Value = "Alertas Activas:";
            worksheet.Cell(fila, 2).Value = AlertasActivas;
            fila++;

            worksheet.Cell(fila, 1).Value = "Alertas Resueltas:";
            worksheet.Cell(fila, 2).Value = AlertasResueltas;
            fila++;

            worksheet.Cell(fila, 1).Value = "Alertas de Proximidad:";
            worksheet.Cell(fila, 2).Value = AlertasProximidad;
            fila++;

            worksheet.Cell(fila, 1).Value = "Alertas de Retraso:";
            worksheet.Cell(fila, 2).Value = AlertasRetraso;

            // Ajustar anchos
            worksheet.Column(1).Width = 10;
            worksheet.Column(2).Width = 20;
            worksheet.Column(3).Width = 15;
            worksheet.Column(4).Width = 12;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 30;
            worksheet.Column(7).Width = 50;
            worksheet.Column(8).Width = 15;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var nombreArchivo = $"HistorialAlertas_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
        }

        /// <summary>
        /// ✨ MÉTODO ADICIONAL: Generar datos de ejemplo directamente desde el dashboard
        /// </summary>
        public async Task<IActionResult> OnPostGenerarDatosEjemploAsync()
        {
            try
            {
                var resultado = await _alertaService.GenerarDatosDeEjemploAsync();

                if (resultado)
                {
                    TempData["Mensaje"] = "Datos de ejemplo generados exitosamente. Se han creado alertas variadas para pruebas.";
                    TempData["TipoMensaje"] = "success";
                }
                else
                {
                    // Si falla, intentar crear algunos buses primero
                    var buses = await _busService.GetAllBusesAsync();
                    if (!buses.Any())
                    {
                        TempData["Mensaje"] = "No hay buses registrados en el sistema. Primero debes crear algunos buses.";
                        TempData["TipoMensaje"] = "warning";
                    }
                    else
                    {
                        TempData["Mensaje"] = "No se pudieron generar los datos de ejemplo. Revisa los logs para más detalles.";
                        TempData["TipoMensaje"] = "error";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error generando datos: {ex.Message}";
                TempData["TipoMensaje"] = "error";
            }

            return RedirectToPage();
        }
    }
}