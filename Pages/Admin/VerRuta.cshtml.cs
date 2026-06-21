using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Models.DB.Usuarios;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class VerRutaModel : PageModel
    {
        private readonly IRutaService _rutaService;
        private readonly IBusService _busService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public VerRutaModel(
            IRutaService rutaService,
            IBusService busService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager)
        {
            _rutaService = rutaService;
            _busService = busService;
            _context = context;
            _userManager = userManager;
        }

        public RutaConParadasDto? Ruta { get; set; }
        public BusDto? Bus { get; set; }
        public string? NombrePiloto { get; set; }
        public string? EmailPiloto { get; set; }
        public List<AlumnoResumen> AlumnosEnRuta { get; set; } = new();
        public List<AlumnoResumen> AlumnosSinParada { get; set; } = new();
        public List<ParadaMapaInfo> ParadasMapa { get; set; } = new();
        public string? MensajeError { get; set; }

        public class AlumnoResumen
        {
            public int IdAlumno { get; set; }
            public string NombreCompleto { get; set; } = string.Empty;
            public string? Direccion { get; set; }
            public bool TieneParada { get; set; }
        }

        public class ParadaMapaInfo
        {
            public int Orden { get; set; }
            public decimal Latitud { get; set; }
            public decimal Longitud { get; set; }
            public string? Direccion { get; set; }
            public string? NombrePadre { get; set; }
            /// <summary>Alumno(s) asignados a esta parada únicamente.</summary>
            public List<string> Hijos { get; set; } = new();
            public bool EsColegio { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int idRuta)
        {
            if (idRuta <= 0)
            {
                MensajeError = "ID de ruta inválido.";
                return Page();
            }

            Ruta = await _rutaService.GetRutaConParadasAsync(idRuta);
            if (Ruta == null)
            {
                MensajeError = $"No se encontró la ruta con ID {idRuta}.";
                return Page();
            }

            Bus = await _busService.GetBusByIdAsync(Ruta.IdBus);

            var asignacion = await _context.AsignacionesPilotoBusDb
                .FirstOrDefaultAsync(a => a.IdBus == Ruta.IdBus && a.EsActual);

            if (asignacion != null)
            {
                var piloto = await _userManager.FindByIdAsync(asignacion.IdUsuarioPiloto);
                if (piloto != null)
                {
                    NombrePiloto = piloto.UserName ?? piloto.Email;
                    EmailPiloto = piloto.Email;
                }
            }

            var alumnosDelBus = await _context.AlumnosDb
                .Where(a => a.IdBusAsignado == Ruta.IdBus)
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .ToListAsync();

            var alumnosConParada = Ruta.Paradas
                .Where(p => p.IdAlumno > 0)
                .Select(p => p.IdAlumno)
                .ToHashSet();

            foreach (var alumno in alumnosDelBus)
            {
                var resumen = new AlumnoResumen
                {
                    IdAlumno = alumno.IdAlumno,
                    NombreCompleto = $"{alumno.Nombre} {alumno.Apellido}",
                    Direccion = alumno.Direccion,
                    TieneParada = alumnosConParada.Contains(alumno.IdAlumno)
                };

                if (resumen.TieneParada)
                    AlumnosEnRuta.Add(resumen);
                else
                    AlumnosSinParada.Add(resumen);
            }

            foreach (var parada in Ruta.Paradas.Where(p => p.IdAlumno <= 0))
            {
                AlumnosEnRuta.Add(new AlumnoResumen
                {
                    IdAlumno = 0,
                    NombreCompleto = "Parada del colegio",
                    Direccion = parada.Direccion,
                    TieneParada = true
                });
            }

            await CargarParadasMapaAsync(idRuta);

            return Page();
        }

        private async Task CargarParadasMapaAsync(int idRuta)
        {
            var paradas = await _context.ParadasDb
                .Where(p => p.IdRuta == idRuta)
                .Include(p => p.Alumno)
                    .ThenInclude(a => a!.Padres)
                .OrderBy(p => p.Orden)
                .ToListAsync();

            foreach (var parada in paradas)
            {
                if (!parada.IdAlumno.HasValue || parada.Alumno == null)
                {
                    ParadasMapa.Add(new ParadaMapaInfo
                    {
                        Orden = parada.Orden,
                        Latitud = parada.Latitud,
                        Longitud = parada.Longitud,
                        Direccion = parada.Direccion,
                        EsColegio = true
                    });
                    continue;
                }

                var alumno = parada.Alumno;
                var padre = alumno.Padres;
                var apellidoAlumno = alumno.Apellido?.Trim() ?? string.Empty;

                ParadasMapa.Add(new ParadaMapaInfo
                {
                    Orden = parada.Orden,
                    Latitud = parada.Latitud,
                    Longitud = parada.Longitud,
                    Direccion = parada.Direccion,
                    NombrePadre = padre != null ? $"{padre.Nombre} {padre.Apellido}" : null,
                    Hijos = new List<string> { $"{alumno.Nombre} {alumno.Apellido}" },
                    EsColegio = false
                });
            }
        }
    }
}
