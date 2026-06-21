using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;

namespace TransportesGenesis.Helpers
{
    public static class MapaTiempoRealQueries
    {
        public class RutaElegibleItem
        {
            public int IdRuta { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string TipoRuta { get; set; } = string.Empty;
            public bool EsActiva { get; set; }
            public string HoraInicio { get; set; } = string.Empty;
            public int TotalParadas { get; set; }
            public bool TurnoActual { get; set; }
        }

        public class BusElegibleItem
        {
            public int IdBus { get; set; }
            public string Placa { get; set; } = string.Empty;
            public int Capacidad { get; set; }
            public int AlumnosAsignados { get; set; }
            public List<RutaElegibleItem> Rutas { get; set; } = new();
        }

        public static async Task<List<BusElegibleItem>> GetBusesElegiblesAsync(ApplicationDbContext context)
        {
            var paradasPorRuta = await context.ParadasDb
                .GroupBy(p => p.IdRuta)
                .Select(g => new { IdRuta = g.Key, Total = g.Count() })
                .ToDictionaryAsync(x => x.IdRuta, x => x.Total);

            var alumnosDirectos = await context.AlumnosDb
                .Where(a => a.IdBusAsignado != null)
                .GroupBy(a => a.IdBusAsignado!.Value)
                .Select(g => new { IdBus = g.Key, Total = g.Count() })
                .ToDictionaryAsync(x => x.IdBus, x => x.Total);

            var alumnosParadaRows = await context.ParadasDb
                .Where(p => p.IdAlumno != null)
                .Join(
                    context.RutasDb,
                    p => p.IdRuta,
                    r => r.IdRuta,
                    (p, r) => new { r.IdBus, IdAlumno = p.IdAlumno!.Value })
                .ToListAsync();

            var alumnosPorParadas = alumnosParadaRows
                .GroupBy(x => x.IdBus)
                .ToDictionary(g => g.Key, g => g.Select(x => x.IdAlumno).Distinct().Count());

            var alumnosPorBus = new Dictionary<int, int>(alumnosDirectos);
            foreach (var item in alumnosPorParadas)
            {
                if (alumnosPorBus.TryGetValue(item.Key, out var actual))
                    alumnosPorBus[item.Key] = Math.Max(actual, item.Value);
                else
                    alumnosPorBus[item.Key] = item.Value;
            }

            var buses = await context.BusesDb
                .OrderBy(b => b.Placa)
                .Select(b => new { b.IdBus, b.Placa, b.Capacidad })
                .ToListAsync();

            var rutas = await context.RutasDb
                .OrderByDescending(r => r.FechaRegistro)
                .Select(r => new
                {
                    r.IdRuta,
                    r.IdBus,
                    r.Nombre,
                    r.TipoRuta,
                    r.EsActiva,
                    r.HoraInicio
                })
                .ToListAsync();

            var turnoSugerido = HorarioRutaHelper.SugerirTurnoActual();
            var resultado = new List<BusElegibleItem>();

            foreach (var bus in buses)
            {
                if (!alumnosPorBus.TryGetValue(bus.IdBus, out var totalAlumnos) || totalAlumnos == 0)
                    continue;

                var rutasBus = rutas
                    .Where(r => r.IdBus == bus.IdBus)
                    .Where(r => paradasPorRuta.TryGetValue(r.IdRuta, out var total) && total > 0)
                    .Select(r => new RutaElegibleItem
                    {
                        IdRuta = r.IdRuta,
                        Nombre = r.Nombre,
                        TipoRuta = r.TipoRuta ?? string.Empty,
                        EsActiva = r.EsActiva,
                        HoraInicio = r.HoraInicio.ToString(@"hh\:mm"),
                        TotalParadas = paradasPorRuta[r.IdRuta],
                        TurnoActual = (r.TipoRuta ?? string.Empty).Contains(turnoSugerido, StringComparison.OrdinalIgnoreCase)
                    })
                    .ToList();

                if (rutasBus.Count == 0)
                    continue;

                resultado.Add(new BusElegibleItem
                {
                    IdBus = bus.IdBus,
                    Placa = bus.Placa,
                    Capacidad = bus.Capacidad,
                    AlumnosAsignados = totalAlumnos,
                    Rutas = rutasBus
                });
            }

            return resultado;
        }
    }
}
