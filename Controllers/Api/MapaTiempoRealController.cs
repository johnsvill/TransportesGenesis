using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Helpers;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("api/mapa-tiempo-real")]
    public class MapaTiempoRealController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IBusService _busService;
        private readonly IRutaService _rutaService;
        private readonly IUbicacionBusService _ubicacionService;

        public MapaTiempoRealController(
            ApplicationDbContext context,
            IBusService busService,
            IRutaService rutaService,
            IUbicacionBusService ubicacionService)
        {
            _context = context;
            _busService = busService;
            _rutaService = rutaService;
            _ubicacionService = ubicacionService;
        }

        [HttpGet("buses-eligibles")]
        public async Task<ActionResult<object>> GetBusesElegibles()
        {
            var data = await MapaTiempoRealQueries.GetBusesElegiblesAsync(_context);
            return Ok(new { buses = data });
        }

        [HttpGet("vista")]
        public async Task<ActionResult<object>> GetVistaBus([FromQuery] int idBus, [FromQuery] int? idRuta)
        {
            var vista = await ConstruirVistaBusAsync(idBus, idRuta);
            if (vista == null)
                return NotFound(new { message = "Bus no encontrado." });

            return Ok(vista);
        }

        private async Task<object?> ConstruirVistaBusAsync(int idBus, int? idRuta)
        {
            var bus = await _busService.GetBusByIdAsync(idBus);
            if (bus == null)
                return null;

            var rutas = (await _rutaService.GetRutasByBusAsync(idBus)).ToList();
            if (rutas.Count == 0)
                return null;

            RutaDto? rutaSeleccionada = null;
            if (idRuta.HasValue)
                rutaSeleccionada = rutas.FirstOrDefault(r => r.IdRuta == idRuta.Value);

            rutaSeleccionada ??= SeleccionarRutaParaVista(rutas);

            if (rutaSeleccionada == null)
                return null;

            var rutaConParadas = await _rutaService.GetRutaConParadasAsync(rutaSeleccionada.IdRuta);
            var ubicacion = await _ubicacionService.GetUltimaUbicacionAsync(idBus);

            var enHorario = HorarioRutaHelper.EstaEnHorarioRecorrido(rutaSeleccionada);
            var gpsReciente = ubicacion != null
                && (DateTime.Now - ubicacion.FechaHora).TotalMinutes <= 10;
            var rutaEnRecorrido = enHorario && gpsReciente;

            var paradas = (rutaConParadas?.Paradas ?? new List<ParadaDto>())
                .OrderBy(p => p.Orden)
                .Select(p => new
                {
                    p.IdParada,
                    p.Orden,
                    p.Latitud,
                    p.Longitud,
                    p.Direccion,
                    p.NombreAlumno,
                    EsColegio = string.IsNullOrWhiteSpace(p.NombreAlumno) || p.NombreAlumno == "Colegio"
                })
                .ToList();

            return new
            {
                modoEnfocado = true,
                idBus = bus.IdBus,
                enHorario,
                rutaEnRecorrido,
                mensajeHorario = enHorario
                    ? null
                    : HorarioRutaHelper.ObtenerMensajeFueraDeHorario(rutaSeleccionada),
                rutasAsignadas = rutas.Count,
                bus = new
                {
                    bus.IdBus,
                    bus.Placa,
                    bus.Modelo,
                    bus.Capacidad,
                    bus.Estado
                },
                ruta = new
                {
                    rutaSeleccionada.IdRuta,
                    rutaSeleccionada.Nombre,
                    rutaSeleccionada.TipoRuta,
                    rutaSeleccionada.EsActiva,
                    rutaSeleccionada.TotalParadas,
                    HoraInicio = rutaSeleccionada.HoraInicio.ToString(@"hh\:mm")
                },
                paradas,
                ubicacion = ubicacion == null ? null : new
                {
                    ubicacion.Latitud,
                    ubicacion.Longitud,
                    ubicacion.Velocidad,
                    ubicacion.FechaHora
                },
                rutasDisponibles = rutas.Select(r => new
                {
                    r.IdRuta,
                    r.Nombre,
                    r.TipoRuta,
                    r.EsActiva,
                    r.TotalParadas,
                    HoraInicio = r.HoraInicio.ToString(@"hh\:mm"),
                    EnHorario = HorarioRutaHelper.EstaEnHorarioRecorrido(r)
                })
            };
        }

        private static RutaDto? SeleccionarRutaParaVista(List<RutaDto> rutas)
        {
            var turno = HorarioRutaHelper.SugerirTurnoActual();

            var porTurnoActiva = rutas
                .Where(r => r.EsActiva && (r.TipoRuta ?? "").Contains(turno, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(r => r.TotalParadas)
                .FirstOrDefault();
            if (porTurnoActiva != null)
                return porTurnoActiva;

            return rutas
                .Where(r => r.TotalParadas > 0)
                .OrderByDescending(r => r.EsActiva)
                .ThenByDescending(r => r.TotalParadas)
                .ThenByDescending(r => r.FechaRegistro)
                .FirstOrDefault()
                ?? rutas.OrderByDescending(r => r.FechaRegistro).FirstOrDefault();
        }
    }
}
