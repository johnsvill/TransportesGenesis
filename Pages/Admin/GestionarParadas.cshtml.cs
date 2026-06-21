using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TransportesGenesis.Data.Context;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class GestionarParadasModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GestionarParadasModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string? Mensaje { get; set; }
        public string TipoMensaje { get; set; } = "info";
        public string ConfigJson { get; set; } = "{}";

        public async Task OnGetAsync(int? idRuta)
        {
            var alumnosPorBus = await _context.AlumnosDb
                .Where(a => a.IdBusAsignado != null)
                .GroupBy(a => a.IdBusAsignado!.Value)
                .Select(g => new { IdBus = g.Key, Total = g.Count() })
                .ToDictionaryAsync(x => x.IdBus, x => x.Total);

            var busesRaw = await _context.BusesDb
                .Where(b => b.Estado)
                .OrderBy(b => b.Placa)
                .Select(b => new { b.IdBus, b.Placa, b.Capacidad })
                .ToListAsync();

            var buses = busesRaw
                .Select(b =>
                {
                    alumnosPorBus.TryGetValue(b.IdBus, out var asignados);
                    return new
                    {
                        b.IdBus,
                        b.Placa,
                        b.Capacidad,
                        AlumnosAsignados = asignados,
                        CuposDisponibles = b.Capacidad - asignados
                    };
                })
                .ToList();

            var rutas = await _context.RutasDb
                .Where(r => r.EsActiva)
                .OrderByDescending(r => r.FechaRegistro)
                .Select(r => new
                {
                    r.IdRuta,
                    r.IdBus,
                    r.Nombre,
                    r.TipoRuta,
                    Fecha = r.FechaRegistro.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            var alumnosConParada = await _context.ParadasDb
                .Where(p => p.IdAlumno != null)
                .Select(p => p.IdAlumno!.Value)
                .Distinct()
                .ToListAsync();

            var alumnosSinAsignar = await _context.AlumnosDb
                .Where(a => a.IdBusAsignado == null && !alumnosConParada.Contains(a.IdAlumno))
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .Select(a => new
                {
                    a.IdAlumno,
                    NombreCompleto = a.Nombre + " " + a.Apellido
                })
                .ToListAsync();

            int? idBusPreseleccionado = null;
            if (idRuta.HasValue)
            {
                var rutaPreseleccionada = await _context.RutasDb
                    .FirstOrDefaultAsync(r => r.IdRuta == idRuta.Value);
                if (rutaPreseleccionada != null)
                    idBusPreseleccionado = rutaPreseleccionada.IdBus;
            }

            ConfigJson = JsonSerializer.Serialize(new
            {
                buses,
                rutas,
                alumnosSinAsignar,
                idRutaPreseleccionada = idRuta,
                idBusPreseleccionado
            }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
