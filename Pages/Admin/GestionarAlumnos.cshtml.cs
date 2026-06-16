using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Repositories.Interfaces;
using PadresEntity = TransportesGenesis.Models.DB.Negocio.Padres;
using AlumnosEntity = TransportesGenesis.Models.DB.Negocio.Alumnos;
using BusEntity = TransportesGenesis.Models.DB.Negocio.Bus;
using ParadaEntity = TransportesGenesis.Models.DB.Negocio.Parada;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class GestionarAlumnosModel : PageModel
    {
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly ApplicationDbContext _context;

        public GestionarAlumnosModel(
            IAlumnoRepository alumnoRepository,
            ApplicationDbContext context)
        {
            _alumnoRepository = alumnoRepository;
            _context = context;
        }

        // Propiedades para la vista
        public List<AlumnoViewModel> Alumnos { get; set; } = new();
        public List<BusEntity> BusesDisponibles { get; set; } = new();
        public List<PadresEntity> PadresDisponibles { get; set; } = new();

        // Propiedades para filtros (Query parameters)
        [BindProperty(SupportsGet = true)]
        public int? FiltroBus { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FiltroEstado { get; set; } // "conbus", "sinbus", "todos"

        [BindProperty(SupportsGet = true)]
        public int? FiltroPadre { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FiltroRutaPadre { get; set; } // "configurada", "noconfigur", "todos"

        // Propiedades para mensajes
        public string? Mensaje { get; set; }
        public string TipoMensaje { get; set; } = "info";

        // Propiedades para los formularios
        [BindProperty]
        public int IdAlumnoAsignar { get; set; }

        [BindProperty]
        public int? IdBusAsignar { get; set; }

        [BindProperty]
        public int IdAlumnoEditar { get; set; }

        [BindProperty]
        public string DireccionEditar { get; set; }

        [BindProperty]
        public double LatitudEditar { get; set; }

        [BindProperty]
        public double LongitudEditar { get; set; }

        public async Task OnGetAsync()
        {
            await CargarDatos();
        }

        public async Task<IActionResult> OnPostAsignarBusAsync()
        {
            try
            {
                // Validar que el alumno existe
                var alumno = await _alumnoRepository.GetByIdAsync(IdAlumnoAsignar);
                if (alumno == null)
                {
                    Mensaje = "El alumno no existe.";
                    TipoMensaje = "danger";
                    await CargarDatos();
                    return Page();
                }

                // Validar que el bus existe si se está asignando
                if (IdBusAsignar.HasValue && IdBusAsignar.Value > 0)
                {
                    var bus = await _context.BusesDb.FindAsync(IdBusAsignar.Value);
                    if (bus == null)
                    {
                        Mensaje = "El bus seleccionado no existe.";
                        TipoMensaje = "danger";
                        await CargarDatos();
                        return Page();
                    }
                }

                // Si se está quitando el bus, eliminar paradas asociadas
                if (!IdBusAsignar.HasValue || IdBusAsignar.Value == 0)
                {
                    await EliminarParadasAlumno(alumno.IdAlumno);
                    alumno.IdBusAsignado = null;
                    await _alumnoRepository.UpdateAsync(alumno);

                    Mensaje = $"Bus removido de {alumno.Nombre} {alumno.Apellido}. Se eliminaron las paradas asociadas.";
                    TipoMensaje = "success";

                    await CargarDatos();
                    return Page();
                }

                // Asignar el bus
                alumno.IdBusAsignado = IdBusAsignar.Value;
                await _alumnoRepository.UpdateAsync(alumno);

                // Si el alumno tiene coordenadas configuradas, crear parada automáticamente
                if (alumno.Latitud.HasValue && alumno.Longitud.HasValue && 
                    alumno.Latitud != 0 && alumno.Longitud != 0 &&
                    !string.IsNullOrEmpty(alumno.Direccion))
                {
                    await CrearParadaAutomatica(alumno);
                    Mensaje = $"Bus asignado exitosamente a {alumno.Nombre} {alumno.Apellido}. Se creó la parada en la ruta activa.";
                }
                else
                {
                    Mensaje = $"Bus asignado exitosamente a {alumno.Nombre} {alumno.Apellido}. Recuerda configurar las coordenadas del alumno para crear la parada en la ruta.";
                }

                TipoMensaje = "success";

                await CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                Mensaje = $"Error al asignar bus: {ex.Message}";
                TipoMensaje = "danger";
                await CargarDatos();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostEditarDireccionAsync()
        {
            try
            {
                // Validar campos
                if (string.IsNullOrWhiteSpace(DireccionEditar))
                {
                    Mensaje = "La dirección es obligatoria.";
                    TipoMensaje = "danger";
                    await CargarDatos();
                    return Page();
                }

                if (LatitudEditar == 0 || LongitudEditar == 0)
                {
                    Mensaje = "Las coordenadas son obligatorias.";
                    TipoMensaje = "danger";
                    await CargarDatos();
                    return Page();
                }

                // Buscar el alumno
                var alumno = await _alumnoRepository.GetByIdAsync(IdAlumnoEditar);
                if (alumno == null)
                {
                    Mensaje = "El alumno no existe.";
                    TipoMensaje = "danger";
                    await CargarDatos();
                    return Page();
                }

                // Actualizar coordenadas
                alumno.Direccion = DireccionEditar;
                alumno.Latitud = (decimal)LatitudEditar;
                alumno.Longitud = (decimal)LongitudEditar;

                await _alumnoRepository.UpdateAsync(alumno);

                // Si el alumno tiene bus asignado, actualizar sus paradas existentes
                if (alumno.IdBusAsignado.HasValue)
                {
                    await ActualizarParadasAlumno(alumno);
                    Mensaje = $"Dirección y coordenadas actualizadas para {alumno.Nombre} {alumno.Apellido}. Se actualizaron las paradas en las rutas.";
                }
                else
                {
                    Mensaje = $"Dirección y coordenadas actualizadas para {alumno.Nombre} {alumno.Apellido}.";
                }

                TipoMensaje = "success";

                await CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                Mensaje = $"Error al editar dirección: {ex.Message}";
                TipoMensaje = "danger";
                await CargarDatos();
                return Page();
            }
        }

        private async Task CargarDatos()
        {
            // Cargar todos los alumnos con includes
            var todosAlumnos = await _alumnoRepository.GetAllWithIncludesAsync();

            // Aplicar filtros
            var alumnosFiltrados = todosAlumnos.AsQueryable();

            // Filtro por bus
            if (FiltroBus.HasValue && FiltroBus.Value > 0)
            {
                alumnosFiltrados = alumnosFiltrados.Where(a => a.IdBusAsignado == FiltroBus.Value);
            }

            // Filtro por estado de asignación
            if (!string.IsNullOrEmpty(FiltroEstado))
            {
                if (FiltroEstado == "conbus")
                {
                    alumnosFiltrados = alumnosFiltrados.Where(a => a.IdBusAsignado.HasValue);
                }
                else if (FiltroEstado == "sinbus")
                {
                    alumnosFiltrados = alumnosFiltrados.Where(a => !a.IdBusAsignado.HasValue);
                }
            }

            // Filtro por padre
            if (FiltroPadre.HasValue && FiltroPadre.Value > 0)
            {
                alumnosFiltrados = alumnosFiltrados.Where(a => a.Padres.IdPadre == FiltroPadre.Value);
            }

            // Convertir a ViewModels y calcular estado de ruta del padre
            Alumnos = alumnosFiltrados.Select(a => new AlumnoViewModel
            {
                IdAlumno = a.IdAlumno,
                Nombre = a.Nombre,
                Apellido = a.Apellido,
                NombrePadre = a.Padres != null ? $"{a.Padres.Nombre} {a.Padres.Apellido}" : "Sin padre",
                IdPadre = a.Padres != null ? a.Padres.IdPadre : 0,
                BusAsignado = a.BusAsignado != null ? $"{a.BusAsignado.Placa} - {a.BusAsignado.Modelo}" : "Sin asignar",
                IdBusAsignado = a.IdBusAsignado,
                TieneCoordenas = a.Latitud.HasValue && a.Longitud.HasValue && a.Latitud != 0 && a.Longitud != 0,
                Latitud = a.Latitud,
                Longitud = a.Longitud,
                Direccion = a.Direccion,
                EstadoCoordenas = (a.Latitud.HasValue && a.Longitud.HasValue && a.Latitud != 0 && a.Longitud != 0)
                    ? "Configurado"
                    : "Sin configurar"
            }).ToList();

            // Calcular estado de ruta del padre para cada alumno
            foreach (var alumno in Alumnos)
            {
                var alumnosPadre = todosAlumnos.Where(a => a.Padres != null && a.Padres.IdPadre == alumno.IdPadre).ToList();
                bool tieneRutaConfigurada = alumnosPadre.Any(a =>
                    a.Latitud.HasValue && a.Longitud.HasValue &&
                    a.Latitud != 0 && a.Longitud != 0 &&
                    a.IdBusAsignado.HasValue);

                alumno.EstadoRutaPadre = tieneRutaConfigurada ? "Configurada" : "No configurada";
            }

            // Filtro por estado de ruta del padre
            if (!string.IsNullOrEmpty(FiltroRutaPadre))
            {
                if (FiltroRutaPadre == "configurada")
                {
                    Alumnos = Alumnos.Where(a => a.EstadoRutaPadre == "Configurada").ToList();
                }
                else if (FiltroRutaPadre == "noconfigur")
                {
                    Alumnos = Alumnos.Where(a => a.EstadoRutaPadre == "No configurada").ToList();
                }
            }

            // Cargar listas para filtros y formularios
            BusesDisponibles = await _context.BusesDb.OrderBy(b => b.Placa).ToListAsync();
            PadresDisponibles = await _context.PadresDb.OrderBy(p => p.Nombre).ToListAsync();
        }

        /// <summary>
        /// Crea una parada automáticamente en la ruta activa del bus asignado al alumno
        /// </summary>
        private async Task CrearParadaAutomatica(AlumnosEntity alumno)
        {
            try
            {
                if (!alumno.IdBusAsignado.HasValue)
                {
                    Console.WriteLine($"[WARN] Alumno {alumno.IdAlumno} no tiene bus asignado.");
                    return;
                }

                // Buscar rutas activas del bus
                var rutasActivas = await _context.RutasDb
                    .Where(r => r.IdBus == alumno.IdBusAsignado.Value && r.EsActiva)
                    .OrderBy(r => r.HoraInicio)
                    .ToListAsync();

                if (!rutasActivas.Any())
                {
                    Console.WriteLine($"[INFO] No hay rutas activas para el bus {alumno.IdBusAsignado.Value}. Se creará la parada cuando exista una ruta activa.");
                    return;
                }

                // Crear parada en cada ruta activa (Mañana y/o Tarde)
                foreach (var ruta in rutasActivas)
                {
                    // Verificar si ya existe una parada para este alumno en esta ruta
                    var paradaExistente = await _context.ParadasDb
                        .FirstOrDefaultAsync(p => p.IdRuta == ruta.IdRuta && p.IdAlumno == alumno.IdAlumno);

                    if (paradaExistente != null)
                    {
                        Console.WriteLine($"[INFO] Ya existe parada para alumno {alumno.IdAlumno} en ruta {ruta.IdRuta}");
                        continue;
                    }

                    // Obtener el último orden de parada
                    var paradas = await _context.ParadasDb
                        .Where(p => p.IdRuta == ruta.IdRuta)
                        .ToListAsync();

                    var ultimoOrden = paradas.Any() ? paradas.Max(p => p.Orden) : 0;

                    // Crear la nueva parada
                    var nuevaParada = new ParadaEntity
                    {
                        IdRuta = ruta.IdRuta,
                        IdAlumno = alumno.IdAlumno,
                        Latitud = alumno.Latitud!.Value,
                        Longitud = alumno.Longitud!.Value,
                        Direccion = alumno.Direccion,
                        Orden = ultimoOrden + 1, // Agregar al final
                        Activo = 1, // 1 = true en int
                        HoraEstimada = null, // Se calculará después
                        Completada = false
                    };

                    _context.ParadasDb.Add(nuevaParada);
                    Console.WriteLine($"[SUCCESS] Parada creada para alumno {alumno.IdAlumno} en ruta {ruta.IdRuta} ({ruta.TipoRuta})");
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al crear parada automática: {ex.Message}");
                // No lanzar excepción para no bloquear la asignación del bus
            }
        }

        /// <summary>
        /// Actualiza las coordenadas de todas las paradas existentes de un alumno
        /// </summary>
        private async Task ActualizarParadasAlumno(AlumnosEntity alumno)
        {
            try
            {
                var paradas = await _context.ParadasDb
                    .Where(p => p.IdAlumno == alumno.IdAlumno)
                    .ToListAsync();

                if (!paradas.Any())
                {
                    // Si no hay paradas existentes pero tiene bus asignado, crear una nueva
                    if (alumno.IdBusAsignado.HasValue)
                    {
                        await CrearParadaAutomatica(alumno);
                    }
                    return;
                }

                // Actualizar coordenadas de todas las paradas existentes
                foreach (var parada in paradas)
                {
                    parada.Latitud = alumno.Latitud!.Value;
                    parada.Longitud = alumno.Longitud!.Value;
                    parada.Direccion = alumno.Direccion;
                }

                await _context.SaveChangesAsync();
                Console.WriteLine($"[SUCCESS] Se actualizaron {paradas.Count} parada(s) del alumno {alumno.IdAlumno}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al actualizar paradas del alumno {alumno.IdAlumno}: {ex.Message}");
                // No lanzar excepción
            }
        }

        /// <summary>
        /// Elimina todas las paradas asociadas a un alumno cuando se le quita el bus
        /// </summary>
        private async Task EliminarParadasAlumno(int idAlumno)
        {
            try
            {
                var paradas = await _context.ParadasDb
                    .Where(p => p.IdAlumno == idAlumno)
                    .ToListAsync();

                if (paradas.Any())
                {
                    _context.ParadasDb.RemoveRange(paradas);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"[SUCCESS] Se eliminaron {paradas.Count} parada(s) del alumno {idAlumno}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al eliminar paradas del alumno {idAlumno}: {ex.Message}");
                // No lanzar excepción
            }
        }

        // ViewModel para la vista
        public class AlumnoViewModel
        {
            public int IdAlumno { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string NombrePadre { get; set; }
            public int IdPadre { get; set; }
            public string BusAsignado { get; set; }
            public int? IdBusAsignado { get; set; }
            public bool TieneCoordenas { get; set; }
            public decimal? Latitud { get; set; }
            public decimal? Longitud { get; set; }
            public string? Direccion { get; set; }
            public string EstadoCoordenas { get; set; }
            public string EstadoRutaPadre { get; set; }
        }
    }
}
