using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using System.Text.Json;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // KPIs Principales
        public int TotalBuses { get; set; }
        public int BusesActivos { get; set; }
        public int TotalRutas { get; set; }
        public int RutasActivas { get; set; }
        public int TotalAlumnos { get; set; }
        public int AlumnosConBus { get; set; }
        public int TotalAlertas { get; set; }
        public int AlertasActivas { get; set; }
        public int TotalPilotos { get; set; }
        public int TotalMonitores { get; set; }
        public int TotalPadres { get; set; }

        // Métricas de Eficiencia
        public decimal PorcentajeBusesActivos { get; set; }
        public decimal PorcentajeRutasActivas { get; set; }
        public decimal PorcentajeAlumnosConBus { get; set; }
        public decimal PromedioParadasPorRuta { get; set; }
        public decimal PromedioAlumnosPorBus { get; set; }

        // Datos para gráficos (JSON)
        public string AsistenciasPorMesJson { get; set; } = "[]";
        public string AlertasPorTipoJson { get; set; } = "[]";
        public string AlumnosPorBusJson { get; set; } = "[]";
        public string RutasPorTipoJson { get; set; } = "[]";

        // Actividad Reciente
        public DateTime? UltimaRutaCreada { get; set; }
        public DateTime? UltimaAlertaGenerada { get; set; }
        public int AsistenciasHoy { get; set; }

        public async Task OnGetAsync()
        {
            await CargarKPIs();
            await CargarMetricas();
            await CargarDatosGraficos();
            await CargarActividadReciente();
        }

        private async Task CargarKPIs()
        {
            // Buses
            var buses = await _context.BusesDb.ToListAsync();
            TotalBuses = buses.Count;
            BusesActivos = buses.Count(b => b.Estado);

            // Rutas
            var rutas = await _context.RutasDb.ToListAsync();
            TotalRutas = rutas.Count;
            RutasActivas = rutas.Count(r => r.EsActiva);

            // Alumnos
            var alumnos = await _context.AlumnosDb.ToListAsync();
            TotalAlumnos = alumnos.Count;
            AlumnosConBus = alumnos.Count(a => a.IdBusAsignado.HasValue);

            // Alertas
            var alertas = await _context.AlertasProximidadDb.ToListAsync();
            TotalAlertas = alertas.Count;
            AlertasActivas = alertas.Count(a => a.Estado == "Activa");

            // Usuarios
            var usuarios = await _context.Users.ToListAsync();
            var userRoles = await _context.UserRoles.ToListAsync();
            var roles = await _context.Roles.ToListAsync();

            var rolePiloto = roles.FirstOrDefault(r => r.Name == "Piloto");
            var roleMonitor = roles.FirstOrDefault(r => r.Name == "Monitor");
            var rolePadre = roles.FirstOrDefault(r => r.Name == "PadreDeFamilia");

            if (rolePiloto != null)
                TotalPilotos = userRoles.Count(ur => ur.RoleId == rolePiloto.Id);
            if (roleMonitor != null)
                TotalMonitores = userRoles.Count(ur => ur.RoleId == roleMonitor.Id);
            if (rolePadre != null)
                TotalPadres = userRoles.Count(ur => ur.RoleId == rolePadre.Id);
        }

        private async Task CargarMetricas()
        {
            // Porcentajes
            PorcentajeBusesActivos = TotalBuses > 0 ? (decimal)BusesActivos / TotalBuses * 100 : 0;
            PorcentajeRutasActivas = TotalRutas > 0 ? (decimal)RutasActivas / TotalRutas * 100 : 0;
            PorcentajeAlumnosConBus = TotalAlumnos > 0 ? (decimal)AlumnosConBus / TotalAlumnos * 100 : 0;

            // Promedio de paradas por ruta
            var totalParadas = await _context.ParadasDb.CountAsync();
            PromedioParadasPorRuta = TotalRutas > 0 ? (decimal)totalParadas / TotalRutas : 0;

            // Promedio de alumnos por bus
            PromedioAlumnosPorBus = TotalBuses > 0 ? (decimal)AlumnosConBus / TotalBuses : 0;
        }

        private async Task CargarDatosGraficos()
        {
            // 1. Asistencias por mes (últimos 6 meses)
            var seisMesesAtras = DateTime.Now.AddMonths(-6);
            var asistenciasPorMesData = await _context.AsistenciasAlumnoDb
                .Where(a => a.Fecha >= seisMesesAtras)
                .GroupBy(a => new { a.Fecha.Year, a.Fecha.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Count()
                })
                .ToListAsync();

            var asistenciasPorMes = asistenciasPorMesData
                .Select(x => new
                {
                    Mes = $"{x.Month}/{x.Year}",
                    Total = x.Total
                })
                .OrderBy(x => x.Mes)
                .ToList();

            AsistenciasPorMesJson = JsonSerializer.Serialize(asistenciasPorMes);

            // 2. Alertas por tipo
            var alertasPorTipo = await _context.AlertasProximidadDb
                .GroupBy(a => a.TipoAlerta)
                .Select(g => new
                {
                    Tipo = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            AlertasPorTipoJson = JsonSerializer.Serialize(alertasPorTipo);

            // 3. Alumnos por bus (top 5 buses con más alumnos)
            var alumnosPorBusData = await _context.AlumnosDb
                .Where(a => a.IdBusAsignado != null)
                .Include(a => a.BusAsignado)
                .ToListAsync();

            var alumnosPorBus = alumnosPorBusData
                .GroupBy(a => a.BusAsignado?.Placa ?? "Sin Bus")
                .Select(g => new
                {
                    Bus = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .ToList();

            AlumnosPorBusJson = JsonSerializer.Serialize(alumnosPorBus);

            // 4. Rutas por tipo (Mañana/Tarde)
            var rutasPorTipo = await _context.RutasDb
                .GroupBy(r => r.TipoRuta)
                .Select(g => new
                {
                    Tipo = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            RutasPorTipoJson = JsonSerializer.Serialize(rutasPorTipo);
        }

        private async Task CargarActividadReciente()
        {
            // Última ruta creada
            var ultimaRuta = await _context.RutasDb
                .OrderByDescending(r => r.FechaRegistro)
                .FirstOrDefaultAsync();
            UltimaRutaCreada = ultimaRuta?.FechaRegistro;

            // Última alerta generada
            var ultimaAlerta = await _context.AlertasProximidadDb
                .OrderByDescending(a => a.FechaHora)
                .FirstOrDefaultAsync();
            UltimaAlertaGenerada = ultimaAlerta?.FechaHora;

            // Asistencias de hoy
            var hoy = DateTime.Now.Date;
            AsistenciasHoy = await _context.AsistenciasAlumnoDb
                .Where(a => a.Fecha.Date == hoy)
                .CountAsync();
        }
    }
}
