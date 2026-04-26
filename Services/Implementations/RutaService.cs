using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public RutaService(
            IRutaRepository rutaRepository,
            IAsistenciaAlumnoRepository asistenciaRepository,
            ISolicitudTrasladoRepository trasladoRepository,
            IConfiguracionService configuracionService,
            IMapper mapper)
        {
            _rutaRepository = rutaRepository;
            _asistenciaRepository = asistenciaRepository;
            _trasladoRepository = trasladoRepository;
            _configuracionService = configuracionService;
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
            Console.WriteLine($"[RUTA SERVICE] Calculando ruta para Bus {dto.IdBus}, Fecha: {dto.Fecha:dd/MM/yyyy}, Tipo: {dto.TipoRuta}");

            // 1. Obtener asistencias confirmadas para la fecha y turno
            var confirmaciones = await _asistenciaRepository.GetConfirmacionesPorFechaAsync(dto.Fecha);
            Console.WriteLine($"[RUTA SERVICE] Total confirmaciones: {confirmaciones.Count()}");

            // 2. Obtener traslados aprobados que afecten esta fecha
            var traslados = await _trasladoRepository.GetTrasladosActivosPorFechaAsync(dto.Fecha);
            Console.WriteLine($"[RUTA SERVICE] Total traslados activos: {traslados.Count()}");

            // 3. Filtrar alumnos del bus considerando traslados
            var alumnosDelBus = new List<Alumnos>();

            foreach (var confirmacion in confirmaciones)
            {
                if (confirmacion.Alumno == null) continue;

                // Verificar si hay traslado aprobado para este alumno en esta fecha
                var trasladoActivo = traslados.FirstOrDefault(t => 
                    t.IdAlumno == confirmacion.IdAlumno && 
                    t.FechaTraslado.Date == dto.Fecha.Date);

                int busAsignado = 0;
                bool debeAsistir = false;

                if (dto.TipoRuta == "Mañana")
                {
                    busAsignado = trasladoActivo?.IdBusDestino ?? confirmacion.IdBusTemporalMañana ?? confirmacion.Alumno.IdBusAsignado ?? 0;
                    debeAsistir = confirmacion.AsisteMañana;
                }
                else // Tarde
                {
                    busAsignado = trasladoActivo?.IdBusDestino ?? confirmacion.IdBusTemporalTarde ?? confirmacion.Alumno.IdBusAsignado ?? 0;
                    debeAsistir = confirmacion.AsisteTarde;
                }

                if (busAsignado == dto.IdBus && debeAsistir && confirmacion.FechaConfirmacion != null)
                {
                    alumnosDelBus.Add(confirmacion.Alumno);
                }
            }

            Console.WriteLine($"[RUTA SERVICE] Alumnos para este bus: {alumnosDelBus.Count}");

            if (!alumnosDelBus.Any())
            {
                Console.WriteLine("[RUTA SERVICE] ⚠️ No hay alumnos confirmados para este bus");
                return new CalcRutaDto
                {
                    IdBus = dto.IdBus,
                    Nombre = $"Ruta {dto.TipoRuta} - Sin alumnos",
                    TipoRuta = dto.TipoRuta,
                    HoraInicio = dto.TipoRuta == "Mañana" ? new TimeSpan(6, 0, 0) : new TimeSpan(14, 0, 0),
                    EsActiva = false,
                    Paradas = new List<ParadaRutaDto>()
                };
            }

            // 4. Calcular ruta óptima usando algoritmo del vecino más cercano (Nearest Neighbor)
            // Incluye el colegio como última parada (mañana) o primera parada (tarde)
            var paradas = await CalcularParadasOptimasAsync(alumnosDelBus, dto.TipoRuta, dto.LatitudInicio, dto.LongitudInicio);

            // 5. Crear la ruta en la BD
            var nuevaRuta = new Ruta
            {
                IdBus = dto.IdBus,
                Nombre = $"Ruta {dto.TipoRuta} - {dto.Fecha:dd/MM/yyyy}",
                Descripcion = $"Ruta calculada automáticamente para {alumnosDelBus.Count} alumnos",
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
                FechaCreacion = nuevaRuta.FechaRegistro
            };

            return resultado;
        }

        public async Task<bool> MarcarParadaCompletadaAsync(MarcarParadaDto dto)
        {
            Console.WriteLine($"[RUTA SERVICE] Marcando parada {dto.IdParada} como completada: {dto.Completada}");
            return await _rutaRepository.MarcarParadaCompletadaAsync(dto.IdParada, dto.Completada);
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
                    // Si IdAlumno es null (colegio), usar "Colegio Genesis"
                    NombreAlumno = p.IdAlumno.HasValue ? (p.Alumno?.Nombre ?? "Desconocido") : "Colegio Genesis",
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
                    NombreAlumno = "Colegio Genesis",
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
                    NombreAlumno = "Colegio Genesis",
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
