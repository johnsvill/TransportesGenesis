using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.Json;
using TransportesGenesis.Models.DB.Usuarios;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;
using TransportesGenesis.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace TransportesGenesis.Pages.Padres
{
    [Authorize(Roles = "PadreDeFamilia")]
    public class DashboardRutaBusAsignadoModel : PageModel
    {
        // ── Datos del padre y alumno ───────────────────────────────────────────
        public string PadreNombre     { get; set; } = string.Empty;
        public string NombreHijo      { get; set; } = "—";

        // ── Datos del bus asignado ─────────────────────────────────────────────
        public int    IdBusAsignado   { get; set; } = 1;   // fallback demo = Bus 1
        public string PlacaBus        { get; set; } = "—";
        public string NombrePiloto    { get; set; } = "—";
        public string RutaNombre      { get; set; } = "—";
        public string HorarioMañana   { get; set; } = "—";
        public string HorarioTarde    { get; set; } = "—";

        // ── Coordenadas de la casa del alumno ──────────────────────────────────
        public decimal? CasaLatitud   { get; set; }
        public decimal? CasaLongitud  { get; set; }

        // ── IDs para SignalR ───────────────────────────────────────────────────
        public List<int> IdsAlumnos   { get; set; } = new();

        // ── Paradas reales del bus (serializado a JSON para el JS) ─────────────
        // Cada elemento: { nombre, lat, lng, orden, idAlumno }
        public string ParadasJson     { get; set; } = "[]";

        // ── Coordenadas del colegio (del bus / última parada) ──────────────────
        public decimal ColegioLatitud  { get; set; } = 14.6270m;
        public decimal ColegioLongitud { get; set; } = -90.5125m;

        // ── Dependencias ──────────────────────────────────────────────────────
        private readonly IAlumnoRepository  _alumnoRepo;
        private readonly IBusService        _busService;
        private readonly IRutaRepository    _rutaRepo;
        private readonly ApplicationDbContext _db;

        public DashboardRutaBusAsignadoModel(
            IAlumnoRepository alumnoRepository,
            IBusService busService,
            IRutaRepository rutaRepository,
            ApplicationDbContext db)
        {
            _alumnoRepo = alumnoRepository;
            _busService  = busService;
            _rutaRepo    = rutaRepository;
            _db          = db;
        }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return;

            try
            {
                // 1. Cargar todos los hijos del padre autenticado
                var hijos = await _alumnoRepo.GetAlumnosByPadreUserIdAsync(userId);
                if (hijos == null || !hijos.Any()) return;

                // 2. Nombre del padre
                var primerHijo = hijos.First();
                if (primerHijo.Padres != null)
                    PadreNombre = $"{primerHijo.Padres.Nombre} {primerHijo.Padres.Apellido}".Trim();

                // 3. Nombre del primer hijo (y todos los ids para SignalR)
                NombreHijo = string.Join(", ", hijos.Select(h => $"{h.Nombre} {h.Apellido}"));
                IdsAlumnos = hijos.Select(h => h.IdAlumno).ToList();

                // 4. Coordenadas de la casa (primer hijo que las tenga)
                var hijoCoordenadas = hijos.FirstOrDefault(h => h.Latitud.HasValue && h.Longitud.HasValue);
                if (hijoCoordenadas != null)
                {
                    CasaLatitud  = hijoCoordenadas.Latitud;
                    CasaLongitud = hijoCoordenadas.Longitud;
                }

                // 5. Bus asignado (tomar el del primer hijo que lo tenga)
                var hijoConBus = hijos.FirstOrDefault(h => h.IdBusAsignado.HasValue);
                if (hijoConBus?.IdBusAsignado == null) return;

                IdBusAsignado = hijoConBus.IdBusAsignado.Value;

                // 6. Info del bus
                var bus = await _busService.GetBusByIdAsync(IdBusAsignado);
                if (bus != null)
                {
                    PlacaBus = bus.Placa;
                    if (!string.IsNullOrEmpty(bus.PilotoAsignado))
                        NombrePiloto = bus.PilotoAsignado;
                }

                // Si PilotoAsignado no vino en el DTO, buscarlo directamente
                if (NombrePiloto == "—")
                {
                    var asignacion = await _db.AsignacionesPilotoBusDb
                        .Where(a => a.IdBus == IdBusAsignado && a.EsActual)
                        .Join(_db.Users,
                              a => a.IdUsuarioPiloto,
                              u => u.Id,
                              (a, u) => new { u.UserName, u.Email })
                        .FirstOrDefaultAsync();

                    if (asignacion != null)
                        NombrePiloto = asignacion.UserName ?? asignacion.Email ?? "—";
                }

                // 7. Rutas activas del bus
                var rutas = (await _rutaRepo.GetActivasByBusAsync(IdBusAsignado)).ToList();

                var rutaMañana = rutas.FirstOrDefault(r =>
                    r.TipoRuta.Contains("Mañana", StringComparison.OrdinalIgnoreCase) ||
                    r.TipoRuta.Contains("Ida",    StringComparison.OrdinalIgnoreCase));

                var rutaTarde = rutas.FirstOrDefault(r =>
                    r.TipoRuta.Contains("Tarde",  StringComparison.OrdinalIgnoreCase) ||
                    r.TipoRuta.Contains("Vuelta", StringComparison.OrdinalIgnoreCase));

                var rutaPrincipal = rutaMañana ?? rutaTarde ?? rutas.FirstOrDefault();

                if (rutaPrincipal != null)
                    RutaNombre = rutaPrincipal.Nombre;

                if (rutaMañana != null)
                    HorarioMañana = rutaMañana.HoraInicio.ToString(@"hh\:mm") + " AM";

                if (rutaTarde != null)
                    HorarioTarde = rutaTarde.HoraInicio.ToString(@"hh\:mm") + " PM";

                // 8. Cargar paradas de la ruta principal
                var paradas = new List<object>();

                if (rutaPrincipal != null)
                {
                    var rutaConParadas = await _rutaRepo.GetConParadasAsync(rutaPrincipal.IdRuta);
                    if (rutaConParadas?.ParadasLink != null && rutaConParadas.ParadasLink.Any())
                        {
                            foreach (var p in rutaConParadas.ParadasLink.OrderBy(p => p.Orden))
                            {
                                // Filtrar coordenadas fuera de Guatemala (lat 13.7-17.9, lon -92.3 a -88.2)
                                if (p.Latitud < 13.7m || p.Latitud > 17.9m ||
                                    p.Longitud < -92.3m || p.Longitud > -88.2m)
                                    continue;

                                paradas.Add(new
                                {
                                    nombre   = p.Alumno != null
                                        ? $"Parada {p.Orden} - {p.Alumno.Nombre} {p.Alumno.Apellido}"
                                        : (p.Direccion ?? $"Parada {p.Orden}"),
                                    lat      = (double)p.Latitud,
                                    lng      = (double)p.Longitud,
                                    orden    = p.Orden,
                                    idAlumno = p.IdAlumno
                                });
                            }

                        // Coordenadas del colegio = última parada sin alumno (o última)
                        var paradaColegio = rutaConParadas.ParadasLink
                            .OrderByDescending(p => p.Orden)
                            .FirstOrDefault(p => p.IdAlumno == null)
                            ?? rutaConParadas.ParadasLink.OrderByDescending(p => p.Orden).First();

                        ColegioLatitud  = paradaColegio.Latitud;
                        ColegioLongitud = paradaColegio.Longitud;
                    }
                }

                // Fallback: si no hay paradas en BD usar coordenadas demo
                if (!paradas.Any())
                {
                    paradas = new List<object>
                    {
                        new { nombre = "7ma Av. y 2da Calle, Zona 4",        lat = 14.6380, lng = -90.5240, orden = 1, idAlumno = (int?)null },
                        new { nombre = "5ta Av. y 4ta Calle, Zona 4",        lat = 14.6358, lng = -90.5215, orden = 2, idAlumno = (int?)null },
                        new { nombre = "Terminal Central, 3ra Av., Zona 4",  lat = 14.6336, lng = -90.5190, orden = 3, idAlumno = (int?)null },
                        new { nombre = "Mercado El Guarda, Zona 4",          lat = 14.6314, lng = -90.5165, orden = 4, idAlumno = (int?)null },
                        new { nombre = "Av. Bolívar y 8va Calle, Zona 4",    lat = 14.6292, lng = -90.5145, orden = 5, idAlumno = (int?)null },
                        new { nombre = "Colegio Yulimay PC, Zona 4",         lat = 14.6270, lng = -90.5125, orden = 6, idAlumno = (int?)null }
                    };
                }

                ParadasJson = JsonSerializer.Serialize(paradas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardPadre] Error cargando datos: {ex.Message}");
            }
        }
    }
}