using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;
using TransportesGenesis.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace TransportesGenesis.Pages.Padres
{
    [Authorize(Roles = "PadreDeFamilia")]
    public class DashboardRutaBusAsignadoModel : PageModel
    {
        public string PadreNombre     { get; set; } = string.Empty;
        public string NombreHijo      { get; set; } = "—";

        public int    IdBusAsignado   { get; set; } = 1;
        public string PlacaBus        { get; set; } = "—";
        public string NombrePiloto    { get; set; } = "—";
        public string RutaNombre      { get; set; } = "—";
        public string HorarioMañana   { get; set; } = "—";
        public string HorarioTarde    { get; set; } = "—";
        public string TipoRutaActiva  { get; set; } = "Mañana";
        public string DescripcionTurno { get; set; } = string.Empty;

        public decimal? CasaLatitud   { get; set; }
        public decimal? CasaLongitud  { get; set; }

        public List<int> IdsAlumnos      { get; set; } = new();
        public string ParadasJson        { get; set; } = "[]";   // segmento Plan B
        public string RutaCompletaJson   { get; set; } = "[]"; // ruta completa del bus (en vivo)
        public string ParadasHijosJson   { get; set; } = "[]"; // paradas/casas de los hijos del padre

        public decimal ColegioLatitud  { get; set; } = 14.6235m;
        public decimal ColegioLongitud { get; set; } = -90.4956m;
        public string ColegioNombre { get; set; } = "Colegio";
        public bool AvisoBusesDistintos { get; set; }
        public string? MensajeBusesDistintos { get; set; }

        private readonly IAlumnoRepository    _alumnoRepo;
        private readonly IBusService          _busService;
        private readonly IRutaRepository      _rutaRepo;
        private readonly IConfiguracionService _configService;
        private readonly ApplicationDbContext _db;

        public DashboardRutaBusAsignadoModel(
            IAlumnoRepository alumnoRepository,
            IBusService busService,
            IRutaRepository rutaRepository,
            IConfiguracionService configService,
            ApplicationDbContext db)
        {
            _alumnoRepo   = alumnoRepository;
            _busService   = busService;
            _rutaRepo     = rutaRepository;
            _configService = configService;
            _db           = db;
        }

        public async Task OnGetAsync(string? turno = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) 
            {
                Console.WriteLine("❌ [PADRE] userId está vacío");
                return;
            }

            Console.WriteLine($"🔵 [PADRE] Cargando dashboard para userId: {userId}");

            try
            {
                // Turno: query ?turno=Mañana|Tarde para demo, o automático por hora
                if (!string.IsNullOrEmpty(turno) &&
                    (turno.Equals("Mañana", StringComparison.OrdinalIgnoreCase) ||
                     turno.Equals("Tarde", StringComparison.OrdinalIgnoreCase)))
                    TipoRutaActiva = turno.Equals("Mañana", StringComparison.OrdinalIgnoreCase) ? "Mañana" : "Tarde";
                else
                    TipoRutaActiva = DateTime.Now.Hour < 12 ? "Mañana" : "Tarde";

                DescripcionTurno = TipoRutaActiva == "Mañana"
                    ? "Mañana: recoge en casas → el colegio es la última parada (destino final)."
                    : "Tarde: sale del colegio (parada 1) → deja alumnos en casa. No regresa al colegio.";

                var hijos = await _alumnoRepo.GetAlumnosByPadreUserIdAsync(userId);

                Console.WriteLine($"🔵 [PADRE] GetAlumnosByPadreUserIdAsync devolvió: {hijos?.Count() ?? 0} hijos");

                if (hijos == null || !hijos.Any()) 
                {
                    Console.WriteLine("❌ [PADRE] No se encontraron hijos para este padre");
                    return;
                }

                var primerHijo = hijos.First();
                if (primerHijo.Padres != null)
                    PadreNombre = $"{primerHijo.Padres.Nombre} {primerHijo.Padres.Apellido}".Trim();

                NombreHijo = string.Join(", ", hijos.Select(h => $"{h.Nombre} {h.Apellido}"));
                IdsAlumnos = hijos.Select(h => h.IdAlumno).ToList();

                Console.WriteLine($"✅ [PADRE] IdsAlumnos cargados: [{string.Join(", ", IdsAlumnos)}]");

                var idsHijos = IdsAlumnos.ToHashSet();

                var hijoCoordenadas = hijos.FirstOrDefault(h => h.Latitud.HasValue && h.Longitud.HasValue);
                if (hijoCoordenadas != null)
                {
                    CasaLatitud  = hijoCoordenadas.Latitud;
                    CasaLongitud = hijoCoordenadas.Longitud;
                }

                var grupoBus = hijos
                    .Where(h => h.IdBusAsignado.HasValue)
                    .GroupBy(h => h.IdBusAsignado!.Value)
                    .OrderByDescending(g => g.Count())
                    .ThenBy(g => g.Key)
                    .FirstOrDefault();

                if (grupoBus == null) return;

                IdBusAsignado = grupoBus.Key;

                var busesDistintos = hijos
                    .Where(h => h.IdBusAsignado.HasValue)
                    .Select(h => h.IdBusAsignado!.Value)
                    .Distinct()
                    .Count();
                if (busesDistintos > 1)
                {
                    AvisoBusesDistintos = true;
                    MensajeBusesDistintos =
                        $"Tus hijos están en {busesDistintos} buses distintos. Se muestra el bus #{IdBusAsignado} (el más frecuente). Contacta al administrador si necesitas ver otro.";
                }

                var bus = await _busService.GetBusByIdAsync(IdBusAsignado);
                if (bus != null)
                {
                    PlacaBus = bus.Placa;
                    if (!string.IsNullOrEmpty(bus.PilotoAsignado))
                        NombrePiloto = bus.PilotoAsignado;
                }

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

                // Colegio desde configuración del sistema
                var colegio = await _configService.ObtenerColegioAsync();
                ColegioLatitud  = colegio.Latitud;
                ColegioLongitud = colegio.Longitud;
                ColegioNombre = colegio.Nombre;

                var rutas = (await _rutaRepo.GetActivasByBusAsync(IdBusAsignado)).ToList();

                var rutaMañana = rutas.FirstOrDefault(r =>
                    r.TipoRuta.Contains("Mañana", StringComparison.OrdinalIgnoreCase) ||
                    r.TipoRuta.Contains("Ida",    StringComparison.OrdinalIgnoreCase));

                var rutaTarde = rutas.FirstOrDefault(r =>
                    r.TipoRuta.Contains("Tarde",  StringComparison.OrdinalIgnoreCase) ||
                    r.TipoRuta.Contains("Vuelta", StringComparison.OrdinalIgnoreCase));

                // Turno activo según hora o parámetro ?turno=
                var rutaPrincipal = (TipoRutaActiva == "Mañana" ? rutaMañana : rutaTarde)
                                    ?? rutaMañana ?? rutaTarde ?? rutas.FirstOrDefault();

                if (rutaPrincipal != null)
                    RutaNombre = rutaPrincipal.Nombre;

                if (rutaMañana != null)
                    HorarioMañana = rutaMañana.HoraInicio.ToString(@"hh\:mm") + " AM";

                if (rutaTarde != null)
                    HorarioTarde = rutaTarde.HoraInicio.ToString(@"hh\:mm") + " PM";

                // [FASE 0.2] Segmento filtrado (Plan B) + ruta completa del bus (en vivo)
                var paradasFiltradas = new List<object>();
                var rutaCompleta     = new List<object>();
                var paradasHijos     = new List<object>();

                if (rutaPrincipal != null)
                {
                    var rutaConParadas = await _rutaRepo.GetConParadasAsync(rutaPrincipal.IdRuta);
                    if (rutaConParadas?.ParadasLink != null && rutaConParadas.ParadasLink.Any())
                    {
                        // Ruta completa del monitor (todas las paradas, sin filtrar)
                        rutaCompleta = rutaConParadas.ParadasLink
                            .OrderBy(p => p.Orden)
                            .Select(p => (object)new
                            {
                                nombre    = p.IdAlumno.HasValue
                                    ? $"{p.Alumno?.Nombre} {p.Alumno?.Apellido}".Trim()
                                    : ColegioNombre,
                                lat       = (double)p.Latitud,
                                lng       = (double)p.Longitud,
                                orden     = p.Orden,
                                idAlumno  = p.IdAlumno,
                                idParada  = p.IdParada,
                                esColegio = !p.IdAlumno.HasValue
                            })
                            .ToList();

                        // Paradas en ruta + casas de todos los hijos de este padre
                        foreach (var hijo in hijos.Where(h => h.IdBusAsignado == IdBusAsignado))
                        {
                            if (hijo.Latitud.HasValue && hijo.Longitud.HasValue)
                            {
                                paradasHijos.Add(new
                                {
                                    nombre   = $"Casa — {hijo.Nombre} {hijo.Apellido}",
                                    lat      = (double)hijo.Latitud.Value,
                                    lng      = (double)hijo.Longitud.Value,
                                    idAlumno = hijo.IdAlumno,
                                    idParada = (int?)null,
                                    esCasa   = true
                                });
                            }

                            var paradaEnRuta = rutaConParadas.ParadasLink
                                .FirstOrDefault(p => p.IdAlumno == hijo.IdAlumno);
                            if (paradaEnRuta != null)
                            {
                                paradasHijos.Add(new
                                {
                                    nombre   = $"{hijo.Nombre} {hijo.Apellido}",
                                    lat      = (double)paradaEnRuta.Latitud,
                                    lng      = (double)paradaEnRuta.Longitud,
                                    idAlumno = hijo.IdAlumno,
                                    idParada = paradaEnRuta.IdParada,
                                    esCasa   = false
                                });
                            }
                        }

                        var paradaHijo = rutaConParadas.ParadasLink
                            .FirstOrDefault(p => p.IdAlumno.HasValue && idsHijos.Contains(p.IdAlumno.Value));

                        var paradaColegio = rutaConParadas.ParadasLink
                            .FirstOrDefault(p => p.IdAlumno == null)
                            ?? rutaConParadas.ParadasLink.OrderByDescending(p => p.Orden).First();

                        ColegioLatitud  = paradaColegio.Latitud;
                        ColegioLongitud = paradaColegio.Longitud;

                        if (TipoRutaActiva == "Mañana")
                        {
                            // Mañana: casa → parada hijo → colegio
                            if (CasaLatitud.HasValue && CasaLongitud.HasValue)
                            {
                                paradasFiltradas.Add(new
                                {
                                    nombre   = $"Casa — {NombreHijo}",
                                    lat      = (double)CasaLatitud.Value,
                                    lng      = (double)CasaLongitud.Value,
                                    orden    = 1,
                                    idAlumno = (int?)IdsAlumnos.FirstOrDefault(),
                                    esCasa   = true
                                });
                            }

                            if (paradaHijo != null)
                            {
                                paradasFiltradas.Add(new
                                {
                                    nombre   = $"Parada — {paradaHijo.Alumno?.Nombre} {paradaHijo.Alumno?.Apellido}",
                                    lat      = (double)paradaHijo.Latitud,
                                    lng      = (double)paradaHijo.Longitud,
                                    orden    = 2,
                                    idAlumno = paradaHijo.IdAlumno,
                                    idParada = paradaHijo.IdParada,
                                    esCasa   = false
                                });
                            }

                            paradasFiltradas.Add(new
                            {
                                nombre   = ColegioNombre,
                                lat      = (double)ColegioLatitud,
                                lng      = (double)ColegioLongitud,
                                orden    = 3,
                                idAlumno = (int?)null,
                                esColegio = true
                            });
                        }
                        else
                        {
                            // Tarde: colegio → parada hijo → casa
                            paradasFiltradas.Add(new
                            {
                                nombre   = ColegioNombre,
                                lat      = (double)ColegioLatitud,
                                lng      = (double)ColegioLongitud,
                                orden    = 1,
                                idAlumno = (int?)null,
                                esColegio = true
                            });

                            if (paradaHijo != null)
                            {
                                paradasFiltradas.Add(new
                                {
                                    nombre   = $"Parada — {paradaHijo.Alumno?.Nombre} {paradaHijo.Alumno?.Apellido}",
                                    lat      = (double)paradaHijo.Latitud,
                                    lng      = (double)paradaHijo.Longitud,
                                    orden    = 2,
                                    idAlumno = paradaHijo.IdAlumno,
                                    idParada = paradaHijo.IdParada,
                                    esCasa   = false
                                });
                            }

                            if (CasaLatitud.HasValue && CasaLongitud.HasValue)
                            {
                                paradasFiltradas.Add(new
                                {
                                    nombre   = $"Casa — {NombreHijo}",
                                    lat      = (double)CasaLatitud.Value,
                                    lng      = (double)CasaLongitud.Value,
                                    orden    = 3,
                                    idAlumno = (int?)IdsAlumnos.FirstOrDefault(),
                                    esCasa   = true
                                });
                            }
                        }
                    }
                }

                // Fallback demo si no hay datos reales
                if (!paradasFiltradas.Any())
                {
                    paradasFiltradas = new List<object>
                    {
                        new { nombre = "Casa (demo)",   lat = 14.6380, lng = -90.5240, orden = 1, idAlumno = (int?)null, esCasa = true },
                        new { nombre = "Parada (demo)", lat = 14.6358, lng = -90.5215, orden = 2, idAlumno = (int?)null },
                        new { nombre = "Colegio (demo)", lat = 14.6270, lng = -90.5125, orden = 3, idAlumno = (int?)null, esColegio = true }
                    };
                }

                if (!rutaCompleta.Any() && paradasFiltradas.Any())
                    rutaCompleta = paradasFiltradas;

                ParadasJson      = JsonSerializer.Serialize(paradasFiltradas);
                RutaCompletaJson = JsonSerializer.Serialize(rutaCompleta);
                ParadasHijosJson = JsonSerializer.Serialize(paradasHijos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardPadre] Error cargando datos: {ex.Message}");
            }
        }
    }
}
