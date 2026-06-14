using AutoMapper;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using TransportesGenesis.Hubs;

namespace TransportesGenesis.Services.Implementations
{
    public class UbicacionBusService : IUbicacionBusService
    {
        private readonly IUbicacionBusRepository _ubicacionRepository;
        private readonly IBusRepository _busRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificacionesHub> _hubContext;
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly IConfiguracionService _configuracionService;

        public UbicacionBusService(
            IUbicacionBusRepository ubicacionRepository,
            IBusRepository busRepository,
            IMapper mapper,
            IHubContext<NotificacionesHub> hubContext,
            IAlumnoRepository alumnoRepository,
            IConfiguracionService configuracionService)
        {
            _ubicacionRepository = ubicacionRepository;
            _busRepository = busRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _alumnoRepository = alumnoRepository;
            _configuracionService = configuracionService;
        }

        public async Task<UbicacionBusDto?> GetUltimaUbicacionAsync(int idBus)
        {
            var ubicacion = await _ubicacionRepository.GetUltimaUbicacionAsync(idBus);
            return ubicacion == null ? null : _mapper.Map<UbicacionBusDto>(ubicacion);
        }

        public async Task<IEnumerable<UbicacionBusEnMapaDto>> GetUbicacionesBusesActivosAsync()
        {
            var ubicaciones = await _ubicacionRepository.GetUltimasUbicacionesBusesActivosAsync();
            var dtos = new List<UbicacionBusEnMapaDto>();

            foreach (var ubicacion in ubicaciones)
            {
                var dto = _mapper.Map<UbicacionBusEnMapaDto>(ubicacion);

                // Calcular estado basado en última actualización
                var minutosDesdeActualizacion = (DateTime.Now - ubicacion.FechaHora).TotalMinutes;

                if (minutosDesdeActualizacion > 10)
                    dto.Estado = "SinSeñal";
                else if (ubicacion.Velocidad.HasValue && ubicacion.Velocidad > 5)
                    dto.Estado = "Movimiento";
                else
                    dto.Estado = "Detenido";

                // Por ahora, alumnos pendientes = 0 (se calculará con ParadaRepository más adelante)
                dto.AlumnosPendientes = 0;

                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<UbicacionBusDto> RegistrarUbicacionAsync(UbicacionBusCreateDto dto)
        {
            // Validar que el bus existe
            var busExiste = await _busRepository.ExistsAsync(dto.IdBus);
            if (!busExiste)
                throw new KeyNotFoundException($"Bus con ID {dto.IdBus} no encontrado");

            var ubicacion = _mapper.Map<UbicacionBusEnTiempoReal>(dto);
            var ubicacionCreada = await _ubicacionRepository.AddAsync(ubicacion);

            return _mapper.Map<UbicacionBusDto>(ubicacionCreada);
        }

        // Extensión: registrar y enviar alertas de proximidad/llegada via SignalR
        public async Task<UbicacionBusDto> RegistrarUbicacionYNotificarAsync(UbicacionBusCreateDto dto)
        {
            var ubicacionDto = await RegistrarUbicacionAsync(dto);

            try
            {
                // Obtener alumnos asignados a este bus
                var alumnos = await _alumnoRepository.GetAlumnosByBusAsync(dto.IdBus);

                // Función auxiliar: distancia Haversine en metros
                static double DistanciaMetros(double lat1, double lon1, double lat2, double lon2)
                {
                    double ToRad(double v) => v * Math.PI / 180.0;
                    var R = 6371000.0; // m
                    var dLat = ToRad(lat2 - lat1);
                    var dLon = ToRad(lon2 - lon1);
                    var a = Math.Sin(dLat/2) * Math.Sin(dLat/2) + Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) * Math.Sin(dLon/2) * Math.Sin(dLon/2);
                    var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
                    return R * c;
                }

                var lat = (double)dto.Latitud;
                var lon = (double)dto.Longitud;

                // Umbrales (metros)
                var umbralProxima = 250.0;
                var umbralLlegadaColegio = 80.0;

                // [FASE 0.6] Coordenadas del colegio desde ConfiguracionService (BD)
                var (colegioLatDec, colegioLonDec) = await _configuracionService.ObtenerCoordenadasColegioAsync();
                var colegioLat = (double)colegioLatDec;
                var colegioLon = (double)colegioLonDec;

                // Verificar proximidad por alumno
                if (alumnos != null && alumnos.Any())
                {
                    foreach (var alumno in alumnos)
                    {
                        if (alumno.Latitud.HasValue && alumno.Longitud.HasValue)
                        {
                            var d = DistanciaMetros((double)alumno.Latitud.Value, (double)alumno.Longitud.Value, lat, lon);
                            if (d <= umbralProxima)
                            {
                                var alertaData = new
                                {
                                    IdAlerta = 0,
                                    IdBus = dto.IdBus,
                                    IdAlumno = alumno.IdAlumno,
                                    TipoAlerta = "proximidad",
                                    Mensaje = $"El bus está próximo a la casa de {alumno.Nombre} {alumno.Apellido}",
                                    ParadasRestantes = (int?)null,
                                    FechaHora = DateTime.Now,
                                    RequiereConfirmacion = true
                                };

                                // Enviar a grupo del bus y al grupo del alumno
                                await _hubContext.Clients.Group($"Bus_{dto.IdBus}").SendAsync("AlertaRecibida", alertaData);
                                await _hubContext.Clients.Group($"Alumno_{alumno.IdAlumno}").SendAsync("AlertaPersonal", alertaData);
                            }
                        }
                    }
                }

                // Verificar llegada al colegio
                var distCole = DistanciaMetros(lat, lon, colegioLat, colegioLon);
                if (distCole <= umbralLlegadaColegio)
                {
                    var alertaColegio = new
                    {
                        IdAlerta = 0,
                        IdBus = dto.IdBus,
                        IdAlumno = (int?)null,
                        TipoAlerta = "llegada_colegio",
                        Mensaje = "El bus ha llegado al colegio.",
                        ParadasRestantes = (int?)null,
                        FechaHora = DateTime.Now,
                        RequiereConfirmacion = false
                    };

                    await _hubContext.Clients.Group($"Bus_{dto.IdBus}").SendAsync("AlertaRecibida", alertaColegio);

                    // También notificar a cada alumno del bus (para padres suscritos por Alumno_id)
                    if (alumnos != null && alumnos.Any())
                    {
                        foreach (var alumno in alumnos)
                        {
                            await _hubContext.Clients.Group($"Alumno_{alumno.IdAlumno}").SendAsync("AlertaPersonal", alertaColegio);
                        }
                    }
                }
                // Broadcast ubicación actual también a grupos de alumnos
                try
                {
                    var ubicacionBroadcast = new
                    {
                        IdBus = dto.IdBus,
                        Latitud = dto.Latitud,
                        Longitud = dto.Longitud,
                        FechaHora = DateTime.Now
                    };

                    // Enviar al grupo del bus
                    await _hubContext.Clients.Group($"Bus_{dto.IdBus}").SendAsync("UbicacionBusActualizada", ubicacionBroadcast);

                    // Enviar a cada alumno asociado (grupo Alumno_{id})
                    if (alumnos != null && alumnos.Any())
                    {
                        foreach (var alumno in alumnos)
                        {
                            await _hubContext.Clients.Group($"Alumno_{alumno.IdAlumno}").SendAsync("UbicacionBusActualizada", ubicacionBroadcast);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UbicacionService] Error broadcasting ubicacion: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // No detener flujo si falla notificación
                Console.WriteLine($"[UbicacionService] Error notificando via SignalR: {ex.Message}");
            }

            return ubicacionDto;
        }

        public async Task<IEnumerable<UbicacionBusDto>> GetHistorialAsync(int idBus, DateTime fechaInicio, DateTime fechaFin)
        {
            var historial = await _ubicacionRepository.GetHistorialAsync(idBus, fechaInicio, fechaFin);
            return _mapper.Map<IEnumerable<UbicacionBusDto>>(historial);
        }

        public async Task<bool> LimpiarHistorialAntiguoAsync(int diasAntiguedad)
        {
            var fechaLimite = DateTime.Now.AddDays(-diasAntiguedad);
            return await _ubicacionRepository.LimpiarHistorialAntiguoAsync(fechaLimite);
        }
    }
}
