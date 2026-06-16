using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClosedXML.Excel;
using System.IO;

namespace TransportesGenesis.Pages.Planner
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnGetExportarExcel()
        {
            // Crear el workbook de Excel
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Planner");

            // Configurar encabezados
            worksheet.Cell(1, 1).Value = "Fase";
            worksheet.Cell(1, 2).Value = "Tarea";
            worksheet.Cell(1, 3).Value = "Fecha Inicio";
            worksheet.Cell(1, 4).Value = "Fecha Fin";
            worksheet.Cell(1, 5).Value = "Estado";
            worksheet.Cell(1, 6).Value = "Porcentaje Avance (%)";

            // Formato de encabezados
            var headerRange = worksheet.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#667eea");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int fila = 2;

            // Sprint 1: Autenticación y Login - 100% Completado
            AgregarTareasSprint(worksheet, ref fila, "Sprint 1: Autenticación y Login", 
                new DateTime(2024, 4, 13), new DateTime(2024, 4, 19), 100, new[]
                {
                    "Scaffold de Identity",
                    "Páginas de Login/Register",
                    "Configuración de Base de Datos"
                });

            // Sprint 2: Módulo de Pagos & Geolocalización - 100% Completado
            AgregarTareasSprint(worksheet, ref fila, "Sprint 2: Módulo de Pagos & Geolocalización",
                new DateTime(2024, 4, 20), new DateTime(2024, 4, 26), 100, new[]
                {
                    "Entidades de Pagos",
                    "Migraciones de Pagos",
                    "Repositorios y Services de Pagos",
                    "Páginas CRUD de Pagos",
                    "Base de Datos Geolocalización",
                    "Backend: DTOs y Repositorios",
                    "Integración de Leaflet.js",
                    "Mapa en Tiempo Real",
                    "Integrar Geolocalización con Login",
                    "Calendario de Asistencias"
                });

            // Sprint 3: SignalR, Rutas & Notificaciones - 100% Completado
            AgregarTareasSprint(worksheet, ref fila, "Sprint 3: SignalR, Rutas & Notificaciones",
                new DateTime(2024, 4, 27), new DateTime(2024, 5, 3), 100, new[]
                {
                    "Hub de SignalR (NotificacionesHub)",
                    "Backend de Rutas Completo",
                    "API REST de Rutas",
                    "Página Mi Ruta para Pilotos",
                    "Integrar SignalR con Login",
                    "Algoritmo TSP para Rutas",
                    "Página Admin: Calcular Rutas"
                });

            // Sprint 4: Calendario de Asistencia - 100% Completado
            AgregarTareasSprint(worksheet, ref fila, "Sprint 4: Calendario de Asistencia",
                new DateTime(2024, 5, 4), new DateTime(2024, 5, 10), 100, new[]
                {
                    "Backend de Asistencia",
                    "API de Asistencia",
                    "Página Confirmar Asistencia",
                    "Validación de Horarios",
                    "Integrar Calendario con Login",
                    "Procesamiento Automático",
                    "Vista del Piloto"
                });

            // Sprint 5: Sistema de Traslados y Pagos - 100% Completado
            AgregarTareasSprint(worksheet, ref fila, "Sprint 5: Sistema de Traslados y Pagos",
                new DateTime(2024, 5, 11), new DateTime(2024, 5, 17), 100, new[]
                {
                    "Backend de Traslados",
                    "API de Traslados",
                    "Página Mis Traslados (Padres)",
                    "Página Gestionar Traslados (Admin)",
                    "Gestión de Usuarios Completa",
                    "Dashboard de Ruta del Bus",
                    "Migraciones de Tabla Pagos",
                    "Validaciones de Pagos",
                    "Integración Login con Pagos"
                });

            // Sprint 6: Sistema de Alertas y Notificaciones - 100% Completado
            AgregarTareasSprint(worksheet, ref fila, "Sprint 6: Sistema de Alertas y Notificaciones",
                new DateTime(2024, 5, 18), new DateTime(2024, 5, 24), 100, new[]
                {
                    "Módulo 1: Base de Datos",
                    "Módulo 2: Servicios y API",
                    "Módulo 3: SignalR Real-time",
                    "Módulo 4: Dashboard Admin",
                    "Módulo 5: Botones Dashboard",
                    "Módulo 6: Datos de Ejemplo",
                    "Alertas de Proximidad",
                    "Alertas de Retraso",
                    "Panel de Notificaciones",
                    "Notificaciones en Tiempo Real",
                    "Gestión de Alertas"
                });

            // Sprint 7: Dashboards de Piloto/Monitor & Testing Final - 100% Completado
            AgregarTareasSprint(worksheet, ref fila, "Sprint 7: Dashboards de Piloto/Monitor & Testing",
                new DateTime(2024, 5, 25), new DateTime(2024, 5, 31), 100, new[]
                {
                    "Vista Mi Ruta (Monitor)",
                    "Listado de Alumnos",
                    "Registro de Asistencia",
                    "Seed Data y Migraciones",
                    "Mapa Interactivo de Paradas",
                    "Dashboard Piloto Base",
                    "Panel de Control Piloto",
                    "Testing de Integración Monitor",
                    "Testing del Sistema de Alertas",
                    "Testing de Integración",
                    "Corrección de Bugs",
                    "Optimización de Performance",
                    "Testing de Seguridad",
                    "Documentación Final",
                    "Guía de Instalación",
                    "Video Demostrativo",
                    "Presentación del Proyecto"
                });

            // Sprint 8: Reportes Finales - 100% Completado
            AgregarTareasSprint(worksheet, ref fila, "Sprint 8: Reportes Finales (OPCIONAL)",
                new DateTime(2024, 6, 1), new DateTime(2024, 6, 7), 100, new[]
                {
                    "Reporte de Asistencias (con filtros y estadísticas)",
                    "Dashboard Analítico Gerencial (KPIs + Chart.js)",
                    "Reporte de Rutas (con exportación Excel)",
                    "Historial de Alertas (con exportación Excel)",
                    "Exportación Excel (Planner, Rutas, Asistencias, Alertas)",
                    "Reporte de Pagos (pendiente integración)"
                });

            // Ajustar anchos de columna
            worksheet.Column(1).Width = 40;
            worksheet.Column(2).Width = 50;
            worksheet.Column(3).Width = 15;
            worksheet.Column(4).Width = 15;
            worksheet.Column(5).Width = 15;
            worksheet.Column(6).Width = 20;

            // Guardar en un stream
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Planner.xlsx");
        }

        private void AgregarTareasSprint(IXLWorksheet worksheet, ref int fila, string nombreSprint, 
            DateTime fechaInicio, DateTime fechaFin, int porcentaje, string[] tareas)
        {
            foreach (var tarea in tareas)
            {
                worksheet.Cell(fila, 1).Value = nombreSprint;
                worksheet.Cell(fila, 2).Value = tarea;
                worksheet.Cell(fila, 3).Value = fechaInicio.ToString("dd/MM/yyyy");
                worksheet.Cell(fila, 4).Value = fechaFin.ToString("dd/MM/yyyy");

                // Determinar estado según porcentaje
                string estado;
                XLColor colorEstado;
                if (porcentaje == 100)
                {
                    estado = "Completado";
                    colorEstado = XLColor.FromHtml("#28a745"); // Verde
                }
                else if (porcentaje > 0)
                {
                    estado = "En progreso";
                    colorEstado = XLColor.FromHtml("#ffc107"); // Amarillo
                }
                else
                {
                    estado = "Pendiente";
                    colorEstado = XLColor.FromHtml("#dc3545"); // Rojo
                }

                worksheet.Cell(fila, 5).Value = estado;
                worksheet.Cell(fila, 5).Style.Fill.BackgroundColor = colorEstado;
                worksheet.Cell(fila, 5).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(fila, 5).Style.Font.Bold = true;
                worksheet.Cell(fila, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Porcentaje con formato
                worksheet.Cell(fila, 6).Value = porcentaje;
                worksheet.Cell(fila, 6).Style.NumberFormat.Format = "0\"%\"";
                worksheet.Cell(fila, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Agregar barra de progreso visual en la celda de porcentaje
                if (porcentaje > 0)
                {
                    var cellPorcentaje = worksheet.Cell(fila, 6);

                    // Color de barra según porcentaje
                    XLColor colorBarra = porcentaje == 100 
                        ? XLColor.FromHtml("#28a745") 
                        : XLColor.FromHtml("#ffc107");

                    // Aplicar color de fondo para simular barra de progreso
                    cellPorcentaje.Style.Fill.BackgroundColor = colorBarra;
                    cellPorcentaje.Style.Font.FontColor = XLColor.White;
                    cellPorcentaje.Style.Font.Bold = true;
                }

                fila++;
            }
        }
    }
}
