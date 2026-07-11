using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.DTOs.Ruta;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;
using GeoRutaDto = TransportesGenesis.DTOs.Geolocalizacion.RutaDto;
using CalcRutaDto = TransportesGenesis.DTOs.Ruta.RutaDto;

namespace TransportesGenesis.Services.Implementations
{
    public class RutaService : IRutaService
    {
        private readonly IRutaRepository _rutaRepository;
        private readonly IAsistenciaAlumnoRepository _asistenciaRepository;
        private readonly ISolicitudTrasladoRepository _trasladoRepository;
        private readonly IConfiguracionService _configuracionService;
        private readonly INotificacionService _notificacionService;
        private readonly ApplicationDbContext _context;
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly IMapper _mapper;

        public RutaService(
            IRutaRepository rutaRepository,
            IAsistenciaAlumnoRepository asistenciaRepository,
            ISolicitudTrasladoRepository trasladoRepository,
            IConfiguracionService configuracionService,
            INotificacionService notificacionService,
            ApplicationDbContext context,
            IAlumnoRepository alumnoRepository,
            IMapper mapper)
        {
            _rutaRepository = rutaRepository;
            _asistenciaRepository = asistenciaRepository;
            _trasladoRepository = trasladoRepository;
            _configuracionService = configuracionService;
            _notificacionService = notificacionService;
            _context = context;
            _alumnoRepository = alumnoRepository;
            _mapper = mapper;
        }

        #region Métodos Existentes (Gestión de Rutas)

        public async Task<IEnumerable<GeoRutaDto>> GetRutasByBusAsync(int idBus)
        {
            var rutas = await _rutaRepository.GetByBusAsync(idBus);
            return _mapper.Map<IEnumerable<GeoRutaDto>>(rutas);
        }

        public async Task<DTOs.Geolocalizacion.RutaConParadasDto?> GetRutaConParadasAsync(int idRuta)
        {
            var ruta = await _rutaRepository.GetConParadasAsync(idRuta);
            return _mapper.Map<DTOs.Geolocalizacion.RutaConParadasDto>(ruta);
        }

        public async Task<IEnumerable<GeoRutaDto>> GetRutasPorTipoAsync(string tipoRuta)
        {
            var rutas = await _rutaRepository.GetByTipoAsync(tipoRuta);
            return _mapper.Map<IEnumerable<GeoRutaDto>>(rutas);
        }

        public async Task<GeoRutaDto> CreateRutaAsync(DTOs.Geolocalizacion.RutaCreateDto dto)
        {
            var ruta = _mapper.Map<Ruta>(dto);
            await _rutaRepository.AddAsync(ruta);
            return _mapper.Map<GeoRutaDto>(ruta);
        }

        public async Task<GeoRutaDto> UpdateRutaAsync(DTOs.Geolocalizacion.RutaUpdateDto dto)
        {
            var ruta = await _rutaRepository.GetByIdAsync(dto.IdRuta);
            if (ruta == null)
                throw new Exception("Ruta no encontrada");

            _mapper.Map(dto, ruta);
            await _rutaRepository.UpdateAsync(ruta);
            return _mapper.Map<GeoRutaDto>(ruta);
        }

        public async Task<bool> DeleteRutaAsync(int idRuta)
        {
            var ruta = await _rutaRepository.GetByIdAsync(idRuta);
            if (ruta == null)
                return false;

            await _rutaRepository.DeleteAsync(idRuta);
            return true;
        }

        public async Task<IEnumerable<GeoRutaDto>> GetRutasDelDiaAsync(DateTime fecha, string tipoRuta)
        {
            var rutas = await _rutaRepository.GetRutasDelDiaAsync(fecha, tipoRuta);
            return _mapper.Map<IEnumerable<GeoRutaDto>>(rutas);
        }

        public async Task<bool> RecalcularRutaAsync(int idRuta)
        {
            // TODO: Implementar lógica de recálculo
            throw new NotImplementedException();
        }

        #endregion

        #region FASE 5: Cálculo Dinámico de Rutas

        public async Task<CalcRutaDto> CalcularRutaOptimizadaAsync(CalcularRutaDto dto)
        {
            // [VALIDACIÓN FECHA] Normalizar a fecha calendario (sin hora) para comparar con AsistenciaAlumno.Fecha
            var fechaRuta = dto.Fecha.Date;
            Console.WriteLine($"[RUTA SERVICE] Calculando ruta para Bus {dto.IdBus}, Fecha: {fechaRuta:dd/MM/yyyy}, Tipo: {dto.TipoRuta}");

            var (alumnosDelBus, mensajeEstado, usoFallback) = await ObtenerAlumnosParaRutaAsync(
                dto.IdBus, fechaRuta, dto.TipoRuta);

            Console.WriteLine($"[RUTA SERVICE] Alumnos para este bus: {alumnosDelBus.Count} (fallback={usoFallback})");

            if (!alumnosDelBus.Any())
            {
                Console.WriteLine("[RUTA SERVICE] ⚠️ Sin alumnos elegibles para este bus/fecha/turno");
                return new CalcRutaDto
                {
                    IdBus = dto.IdBus,
                    Nombre = $"Ruta {dto.TipoRuta} - Sin alumnos",
                    TipoRuta = dto.TipoRuta,
                    HoraInicio = dto.TipoRuta == "Mañana" ? new TimeSpan(6, 0, 0) : new TimeSpan(14, 0, 0),
                    EsActiva = false,
                    Paradas = new List<ParadaRutaDto>(),
                    MensajeEstado = mensajeEstado
                };
            }

            // Filtrar alumnos sin coordenadas GPS (requeridas para calcular paradas)
            var alumnosConGps = alumnosDelBus
                .Where(a => a.Latitud.HasValue && a.Longitud.HasValue)
                .ToList();

            if (!alumnosConGps.Any())
            {
                return new CalcRutaDto
                {
                    IdBus = dto.IdBus,
                    Nombre = $"Ruta {dto.TipoRuta} - Sin GPS",
                    TipoRuta = dto.TipoRuta,
                    HoraInicio = dto.TipoRuta == "Mañana" ? new TimeSpan(6, 0, 0) : new TimeSpan(14, 0, 0),
                    EsActiva = false,
                    Paradas = new List<ParadaRutaDto>(),
                    MensajeEstado = $"Hay {alumnosDelBus.Count} alumno(s) en el bus {dto.IdBus}, pero ninguno tiene coordenadas GPS. Configure la dirección del alumno primero."
                };
            }

            var paradas = await CalcularParadasOptimasAsync(alumnosConGps, dto.TipoRuta, dto.LatitudInicio, dto.LongitudInicio);

            // 5. Crear la ruta en la BD
            var nuevaRuta = new Ruta
            {
                IdBus = dto.IdBus,
                Nombre = $"Ruta {dto.TipoRuta} - {fechaRuta:dd/MM/yyyy}",
                Descripcion = $"Ruta calculada automáticamente para {alumnosConGps.Count} alumnos",
                TipoRuta = dto.TipoRuta,
                HoraInicio = dto.TipoRuta == "Mañana" ? new TimeSpan(6, 0, 0) : new TimeSpan(14, 0, 0),
                EsActiva = true,
                FechaRegistro = DateTime.Now
            };

            await _rutaRepository.AddAsync(nuevaRuta);

            // 6. Guardar las paradas
            foreach (var parada in paradas)
            {
                var nuevaParada = new Parada
                {
                    IdRuta = nuevaRuta.IdRuta,
                    IdAlumno = parada.IdAlumno,
                    Latitud = parada.Latitud,
                    Longitud = parada.Longitud,
                    Direccion = parada.Direccion,
                    Orden = parada.Orden,
                    HoraEstimada = parada.HoraEstimada,
                    Completada = false,
                    FechaRegistro = DateTime.Now
                };

                nuevaRuta.ParadasLink.Add(nuevaParada);
            }

            await _rutaRepository.UpdateAsync(nuevaRuta);

            Console.WriteLine($"[RUTA SERVICE] ✅ Ruta creada con ID: {nuevaRuta.IdRuta}, {paradas.Count} paradas");

            // 7. Mapear las paradas guardadas (ahora con IdParada asignado por la BD)
            var paradasGuardadas = nuevaRuta.ParadasLink
                .OrderBy(p => p.Orden)
                .Select(p => new ParadaRutaDto
                {
                    IdParada = p.IdParada,
                    IdAlumno = p.IdAlumno,
                    NombreAlumno = paradas.FirstOrDefault(pa => pa.IdAlumno == p.IdAlumno)?.NombreAlumno ?? "Desconocido",
                    Latitud = p.Latitud,
                    Longitud = p.Longitud,
                    Direccion = p.Direccion,
                    Orden = p.Orden,
                    HoraEstimada = p.HoraEstimada,
                    Completada = p.Completada
                })
                .ToList();

            var mensajeExito = usoFallback
                ? $"Ruta generada con {alumnosConGps.Count} alumno(s) asignados al bus (sin confirmación de asistencia para {fechaRuta:dd/MM/yyyy}). {mensajeEstado}"
                : $"Ruta calculada con {alumnosConGps.Count} alumno(s) con asistencia confirmada.";

            // 8. Retornar resultado con IdParada correcto
            var resultado = new CalcRutaDto
            {
                IdRuta = nuevaRuta.IdRuta,
                IdBus = nuevaRuta.IdBus,
                PlacaBus = $"BUS-{dto.IdBus:D3}",
                Nombre = nuevaRuta.Nombre,
                Descripcion = nuevaRuta.Descripcion,
                TipoRuta = nuevaRuta.TipoRuta,
                HoraInicio = nuevaRuta.HoraInicio,
                EsActiva = nuevaRuta.EsActiva,
                Paradas = paradasGuardadas,
                FechaCreacion = nuevaRuta.FechaRegistro,
                MensajeEstado = mensajeExito
            };

            return resultado;
        }

        /// <summary>
        /// [CONFIRMACIÓN ASISTENCIA] Obtiene alumnos elegibles para calcular la ruta.
        /// 1) Prioridad: asistencia confirmada (FechaConfirmacion != null) para la fecha exacta y turno.
        /// 2) Fallback demo/admin: alumnos asignados al bus (IdBusAsignado) si no hay confirmaciones ese día.
        /// </summary>
        private async Task<(List<Alumnos> alumnos, string mensaje, bool usoFallback)> ObtenerAlumnosParaRutaAsync(
            int idBus, DateTime fechaRuta, string tipoRuta)
        {
            var esMañana = tipoRuta.Equals("Mañana", StringComparison.OrdinalIgnoreCase);

            // Paso 1: asistencias confirmadas para la fecha (tabla genesis.AsistenciaAlumno)
            var confirmaciones = await _asistenciaRepository.GetConfirmacionesPorFechaAsync(fechaRuta);
            var traslados = await _trasladoRepository.GetTrasladosActivosPorFechaAsync(fechaRuta);

            var alumnosConfirmados = new List<Alumnos>();

            foreach (var confirmacion in confirmaciones)
            {
                if (confirmacion.Alumno == null) continue;

                var trasladoActivo = traslados.FirstOrDefault(t =>
                    t.IdAlumno == confirmacion.IdAlumno &&
                    t.FechaTraslado.Date == fechaRuta);

                int busAsignado;
                bool debeAsistir;

                if (esMañana)
                {
                    busAsignado = trasladoActivo?.IdBusDestino ?? confirmacion.IdBusTemporalMañana ?? confirmacion.Alumno.IdBusAsignado ?? 0;
                    debeAsistir = confirmacion.AsisteMañana;
                }
                else
                {
                    busAsignado = trasladoActivo?.IdBusDestino ?? confirmacion.IdBusTemporalTarde ?? confirmacion.Alumno.IdBusAsignado ?? 0;
                    debeAsistir = confirmacion.AsisteTarde;
                }

                if (busAsignado == idBus && debeAsistir && confirmacion.FechaConfirmacion != null)
                    alumnosConfirmados.Add(confirmacion.Alumno);
            }

            if (alumnosConfirmados.Any())
            {
                return (alumnosConfirmados, string.Empty, false);
            }

            // Paso 2: fallback — alumnos permanentemente asignados al bus
            var asignadosAlBus = (await _alumnoRepository.GetAlumnosByBusAsync(idBus))
                .Where(a => a.Activo == 1)
                .ToList();

            if (!asignadosAlBus.Any())
            {
                return (alumnosConfirmados,
                    $"No hay alumnos asignados al bus {idBus}. Asigne alumnos al bus o confirme asistencia para el {fechaRuta:dd/MM/yyyy} (turno {tipoRuta}). Puede ejecutar Scripts/Seed_Escenarios_Completos.sql.",
                    false);
            }

            // Si hay registro de asistencia sin confirmar, respetar AsisteMañana/AsisteTarde
            var alumnosFallback = new List<Alumnos>();
            foreach (var alumno in asignadosAlBus)
            {
                var asistencia = await _asistenciaRepository.GetByAlumnoYFechaAsync(alumno.IdAlumno, fechaRuta);
                if (asistencia == null)
                {
                    // Sin registro para esa fecha: incluir en fallback (demo/admin)
                    alumnosFallback.Add(alumno);
                }
                else if (asistencia.FechaConfirmacion == null)
                {
                    // Registro existe pero padre no confirmó: incluir igualmente en fallback admin
                    if ((esMañana && asistencia.AsisteMañana) || (!esMañana && asistencia.AsisteTarde))
                        alumnosFallback.Add(alumno);
                }
                // Si FechaConfirmacion != null pero no entró arriba, el turno o bus no coincidió — no incluir
            }

            if (alumnosFallback.Any())
            {
                return (alumnosFallback,
                    $"Sin asistencia confirmada para {fechaRuta:dd/MM/yyyy}. Se usaron alumnos asignados al bus.",
                    true);
            }

            return (alumnosConfirmados,
                $"Hay {asignadosAlBus.Count} alumno(s) en el bus {idBus}, pero ninguno confirmó asistencia para el {fechaRuta:dd/MM/yyyy} (turno {tipoRuta}). Confirme desde el portal de padres o ejecute Scripts/Seed_Escenarios_Completos.sql.",
                false);
        }

        public async Task<bool> MarcarParadaCompletadaAsync(MarcarParadaDto dto, string? confirmadoPor = null)
        {
            Console.WriteLine($"[RUTA SERVICE] Marcando parada {dto.IdParada} como completada: {dto.Completada}");

            var parada = await _rutaRepository.GetParadaByIdAsync(dto.IdParada);
            if (parada == null)
                return false;

            var ok = await _rutaRepository.MarcarParadaCompletadaAsync(dto.IdParada, dto.Completada);
            if (!ok || !dto.Completada)
                return ok;

            var tipoRuta = parada.Ruta?.TipoRuta ?? "Mañana";
            var esParadaColegio = !parada.IdAlumno.HasValue;

            // [FASE 0.4] RegistroRecogida + SignalR para paradas con alumno
            if (parada.IdAlumno.HasValue && parada.Alumno != null)
            {
                var registro = new RegistroRecogida
                {
                    Parada = parada,
                    Alumno = parada.Alumno,
                    FechaHoraRecogida = dto.HoraCompletada ?? DateTime.Now,
                    ConfirmadoPor = confirmadoPor,
                    Latitud = parada.Latitud,
                    Longitud = parada.Longitud,
                    AlumnoPresente = true,
                    FechaRegistro = DateTime.Now,
                    Activo = 1
                };
                _context.RegistrosRecogidaDb.Add(registro);
                await _context.SaveChangesAsync();

                await _notificacionService.NotificarParadaCompletadaAsync(
                    parada.IdAlumno.Value,
                    $"{parada.Alumno.Nombre} {parada.Alumno.Apellido}".Trim(),
                    parada.IdParada,
                    tipoRuta,
                    esParadaColegio: false);
            }
            else if (esParadaColegio && tipoRuta.Equals("Mañana", StringComparison.OrdinalIgnoreCase))
            {
                // Parada del colegio en ruta de mañana: notificar a todos los alumnos de esa ruta
                var paradasAlumnos = await _context.Set<Parada>()
                    .Include(p => p.Alumno)
                    .Where(p => p.IdRuta == parada.IdRuta && p.IdAlumno.HasValue && p.Alumno != null)
                    .ToListAsync();

                foreach (var pAlumno in paradasAlumnos)
                {
                    await _notificacionService.NotificarParadaCompletadaAsync(
                        pAlumno.IdAlumno!.Value,
                        $"{pAlumno.Alumno!.Nombre} {pAlumno.Alumno.Apellido}".Trim(),
                        parada.IdParada,
                        tipoRuta,
                        esParadaColegio: true);
                }
            }

            return true;
        }

        public async Task<CalcRutaDto?> GetRutaActivaDelBusAsync(int idBus, DateTime fecha, string tipoRuta)
        {
            var rutas = await _rutaRepository.GetRutasDelDiaAsync(fecha, tipoRuta);
            var rutaDelBus = rutas.FirstOrDefault(r => r.IdBus == idBus);

            if (rutaDelBus == null)
                return null;

            var paradas = rutaDelBus.ParadasLink
                .OrderBy(p => p.Orden)
                .Select(p => new ParadaRutaDto
                {
                    IdParada = p.IdParada,
                    IdAlumno = p.IdAlumno,
                    // Si IdAlumno es null (colegio), usar nombre configurado
                    NombreAlumno = p.IdAlumno.HasValue ? (p.Alumno?.Nombre ?? "Desconocido") : "Colegio",
                    Latitud = p.Latitud,
                    Longitud = p.Longitud,
                    Direccion = p.Direccion,
                    Orden = p.Orden,
                    HoraEstimada = p.HoraEstimada,
                    Completada = p.Completada
                })
                .ToList();

            return new CalcRutaDto
            {
                IdRuta = rutaDelBus.IdRuta,
                IdBus = rutaDelBus.IdBus,
                PlacaBus = rutaDelBus.Bus?.Placa ?? $"BUS-{idBus:D3}",
                Nombre = rutaDelBus.Nombre,
                Descripcion = rutaDelBus.Descripcion,
                TipoRuta = rutaDelBus.TipoRuta,
                HoraInicio = rutaDelBus.HoraInicio,
                EsActiva = rutaDelBus.EsActiva,
                Paradas = paradas,
                FechaCreacion = rutaDelBus.FechaRegistro
            };
        }

        #endregion

        #region Algoritmo de Optimización de Rutas

        private async Task<List<ParadaRutaDto>> CalcularParadasOptimasAsync(List<Alumnos> alumnos, string tipoRuta, decimal? latInicio, decimal? lonInicio)
        {
            Console.WriteLine($"[ALGORITMO] Calculando orden óptimo para {alumnos.Count} alumnos");

            var paradas = new List<ParadaRutaDto>();
            var alumnosPendientes = alumnos.ToList();
            var orden = 1;

            // Obtener coordenadas del colegio
            var (latColegio, lonColegio) = await _configuracionService.ObtenerCoordenadasColegioAsync();
            var direccionColegio = await _configuracionService.ObtenerDireccionColegioAsync();
            var colegioConfig = await _configuracionService.ObtenerColegioAsync();
            var nombreColegio = colegioConfig.Nombre;
            Console.WriteLine($"[ALGORITMO] Colegio en: {latColegio}, {lonColegio} - {direccionColegio}");

            // Determinar punto de inicio según turno
            decimal latActual, lonActual;
            var horaActual = new TimeSpan(6, 0, 0);

            if (tipoRuta == "Mañana")
            {
                // Mañana: Inicia en ubicación del bus o primer alumno
                latActual = latInicio ?? alumnos[0].Latitud ?? 0;
                lonActual = lonInicio ?? alumnos[0].Longitud ?? 0;
                horaActual = new TimeSpan(6, 0, 0);
                Console.WriteLine($"[ALGORITMO] Ruta MAÑANA: Inicio en posición del bus → Alumnos → Colegio (destino final)");
            }
            else
            {
                // Tarde: Inicia en el COLEGIO
                latActual = latColegio;
                lonActual = lonColegio;
                horaActual = new TimeSpan(14, 30, 0);
                Console.WriteLine($"[ALGORITMO] Ruta TARDE: Inicio en Colegio → Alumnos (en orden de cercanía)");

                // Agregar parada del colegio como PRIMERA parada en ruta de tarde
                paradas.Add(new ParadaRutaDto
                {
                    IdAlumno = null, // null indica que es el colegio (sin alumno asignado)
                    NombreAlumno = nombreColegio,
                    Latitud = latColegio,
                    Longitud = lonColegio,
                    Direccion = direccionColegio,
                    Orden = orden++,
                    HoraEstimada = horaActual,
                    Completada = false
                });
                Console.WriteLine($"[ALGORITMO] Parada {orden - 1}: COLEGIO (punto de inicio) - Hora: {horaActual}");
            }

            // Algoritmo del vecino más cercano para los alumnos
            while (alumnosPendientes.Any())
            {
                // Encontrar el alumno más cercano al punto actual
                Alumnos alumnoMasCercano = null;
                double distanciaMinima = double.MaxValue;

                foreach (var alumno in alumnosPendientes)
                {
                    if (alumno.Latitud == null || alumno.Longitud == null)
                        continue;

                    var distancia = CalcularDistancia(latActual, lonActual, alumno.Latitud.Value, alumno.Longitud.Value);

                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        alumnoMasCercano = alumno;
                    }
                }

                if (alumnoMasCercano == null)
                    break;

                // Agregar la parada del alumno
                paradas.Add(new ParadaRutaDto
                {
                    IdAlumno = alumnoMasCercano.IdAlumno,
                    NombreAlumno = alumnoMasCercano.Nombre,
                    Latitud = alumnoMasCercano.Latitud.Value,
                    Longitud = alumnoMasCercano.Longitud.Value,
                    Direccion = alumnoMasCercano.Direccion ?? "Dirección no especificada",
                    Orden = orden++,
                    HoraEstimada = horaActual,
                    Completada = false
                });

                // Actualizar posición actual
                latActual = alumnoMasCercano.Latitud.Value;
                lonActual = alumnoMasCercano.Longitud.Value;

                // Estimar tiempo: ~3 minutos por kilómetro + 2 minutos por parada
                var tiempoViaje = TimeSpan.FromMinutes((distanciaMinima * 3) + 2);
                horaActual = horaActual.Add(tiempoViaje);

                alumnosPendientes.Remove(alumnoMasCercano);
                Console.WriteLine($"[ALGORITMO] Parada {orden - 1}: {alumnoMasCercano.Nombre} - Distancia: {distanciaMinima:F2} km");
            }

            // Si es ruta de MAÑANA, agregar el colegio como ÚLTIMA parada
            if (tipoRuta == "Mañana")
            {
                var distanciaAlColegio = CalcularDistancia(latActual, lonActual, latColegio, lonColegio);
                var tiempoAlColegio = TimeSpan.FromMinutes((distanciaAlColegio * 3) + 2);
                horaActual = horaActual.Add(tiempoAlColegio);

                paradas.Add(new ParadaRutaDto
                {
                    IdAlumno = null, // null indica que es el colegio (sin alumno asignado)
                    NombreAlumno = nombreColegio,
                    Latitud = latColegio,
                    Longitud = lonColegio,
                    Direccion = direccionColegio,
                    Orden = orden++,
                    HoraEstimada = horaActual,
                    Completada = false
                });
                Console.WriteLine($"[ALGORITMO] Parada FINAL {orden - 1}: COLEGIO (destino) - Distancia: {distanciaAlColegio:F2} km - Hora estimada llegada: {horaActual}");
            }

            Console.WriteLine($"[ALGORITMO] ✅ Ruta optimizada con {paradas.Count} paradas ({tipoRuta})");
            return paradas;
        }

        /// <summary>
        /// Calcula la distancia en kilómetros entre dos puntos GPS usando la fórmula del Haversine
        /// </summary>
        private double CalcularDistancia(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            const double R = 6371; // Radio de la Tierra en kilómetros

            var dLat = ToRadians((double)(lat2 - lat1));
            var dLon = ToRadians((double)(lon2 - lon1));

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians((double)lat1)) * Math.Cos(ToRadians((double)lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distancia = R * c;

            return distancia;
        }

        private double ToRadians(double grados)
        {
            return grados * Math.PI / 180;
        }

        #endregion
    }
}
