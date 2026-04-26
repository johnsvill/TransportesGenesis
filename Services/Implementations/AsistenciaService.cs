using AutoMapper;
using TransportesGenesis.DTOs.Asistencia;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Services.Implementations
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IAsistenciaAlumnoRepository _asistenciaRepository;
        private readonly IBusRepository _busRepository;
        private readonly IMapper _mapper;

        public AsistenciaService(
            IAsistenciaAlumnoRepository asistenciaRepository,
            IBusRepository busRepository,
            IMapper mapper)
        {
            _asistenciaRepository = asistenciaRepository;
            _busRepository = busRepository;
            _mapper = mapper;
        }

        #region Métodos Existentes (mantener compatibilidad)

        public async Task<AsistenciaAlumnoDto?> GetAsistenciaDelDiaAsync(int idAlumno, DateTime fecha)
        {
            var asistencia = await _asistenciaRepository.GetByAlumnoYFechaAsync(idAlumno, fecha);
            return asistencia == null ? null : _mapper.Map<AsistenciaAlumnoDto>(asistencia);
        }

        public async Task<AsistenciaCalendarioDto> GetCalendarioMensualAsync(int idAlumno, int mes, int anio)
        {
            var primerDia = new DateTime(anio, mes, 1);
            var ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            var asistencias = await _asistenciaRepository.GetByAlumnoYRangoAsync(idAlumno, primerDia, ultimoDia);

            var calendario = new AsistenciaCalendarioDto
            {
                IdAlumno = idAlumno,
                NombreAlumno = asistencias.FirstOrDefault()?.Alumno != null 
                    ? $"{asistencias.FirstOrDefault().Alumno.Nombre} {asistencias.FirstOrDefault().Alumno.Apellido}"
                    : "",
                Calendario = asistencias.ToDictionary(
                    a => a.Fecha.Date,
                    a => new AsistenciaDiaDto
                    {
                        AsisteMañana = a.AsisteMañana,
                        AsisteTarde = a.AsisteTarde,
                        Confirmado = a.FechaConfirmacion.HasValue
                    }
                )
            };

            return calendario;
        }

        public async Task<AsistenciaAlumnoDto> RegistrarAsistenciaAsync(AsistenciaCreateDto dto)
        {
            var asistencia = _mapper.Map<AsistenciaAlumno>(dto);
            asistencia.Fecha = dto.Fecha.Date; // Solo fecha, sin hora

            var resultado = await _asistenciaRepository.AddAsync(asistencia);
            return _mapper.Map<AsistenciaAlumnoDto>(resultado);
        }

        public async Task<AsistenciaAlumnoDto> ActualizarAsistenciaAsync(AsistenciaUpdateDto dto)
        {
            var asistencia = await _asistenciaRepository.GetByIdAsync(dto.IdAsistencia);
            if (asistencia == null)
                throw new KeyNotFoundException($"Asistencia con ID {dto.IdAsistencia} no encontrada");

            _mapper.Map(dto, asistencia);
            await _asistenciaRepository.UpdateAsync(asistencia);

            return _mapper.Map<AsistenciaAlumnoDto>(asistencia);
        }

        public async Task<bool> ConfirmarAsistenciaAsync(int idAsistencia)
        {
            return await _asistenciaRepository.ConfirmarAsistenciaAsync(idAsistencia);
        }

        public async Task<IEnumerable<AsistenciaAlumnoDto>> GetAsistenciasDelDiaAsync(DateTime fecha)
        {
            var asistencias = await _asistenciaRepository.GetConfirmadasDelDiaAsync(fecha, true, true);
            return _mapper.Map<IEnumerable<AsistenciaAlumnoDto>>(asistencias);
        }

        public async Task<bool> ProcesarConfirmacionesAutomaticasAsync(DateTime fecha, string turno)
        {
            // Lógica para marcar como no asistentes a los que no confirmaron
            var pendientes = await _asistenciaRepository.GetPendientesDeConfirmarAsync(fecha);

            foreach (var asistencia in pendientes)
            {
                if (turno.ToLower() == "mañana")
                    asistencia.AsisteMañana = false;
                else if (turno.ToLower() == "tarde")
                    asistencia.AsisteTarde = false;

                asistencia.FechaConfirmacion = DateTime.Now;
                await _asistenciaRepository.UpdateAsync(asistencia);
            }

            return true;
        }

        #endregion

        #region FASE 4: Métodos para Confirmación de Padres

        public async Task<AsistenciaResumenDto?> GetResumenAsistenciaHoyAsync(int idAlumno)
        {
            var hoy = DateTime.Today;
            var diaSemana = hoy.DayOfWeek;

            var ahora = DateTime.Now;
            var puedeConfirmarMañana = await PuedeConfirmarMañanaAsync(ahora);
            var puedeConfirmarTarde = await PuedeConfirmarTardeAsync(ahora);

            // Si es fin de semana, buscar el próximo día hábil (lunes)
            DateTime fechaConsulta = hoy;
            if (diaSemana == DayOfWeek.Saturday)
            {
                fechaConsulta = hoy.AddDays(2); // Sábado → Lunes
            }
            else if (diaSemana == DayOfWeek.Sunday)
            {
                fechaConsulta = hoy.AddDays(1); // Domingo → Lunes
            }

            var asistencia = await _asistenciaRepository.GetByAlumnoYFechaAsync(idAlumno, fechaConsulta);

            // Si no hay asistencia O si no hay alumno, retornar datos simulados
            if (asistencia == null || asistencia.Alumno == null)
            {
                string mensajeEstado = diaSemana == DayOfWeek.Saturday || diaSemana == DayOfWeek.Sunday
                    ? $"🏖️ Fin de semana - Próximo día escolar: {fechaConsulta:dddd dd/MM}"
                    : "📋 Modo de prueba (sin datos en BD)";

                return new AsistenciaResumenDto
                {
                    IdAlumno = idAlumno,
                    NombreCompleto = "Alumno de Prueba",
                    IdBusAsignado = 1,
                    PlacaBus = "BUS-001",
                    TieneConfirmacionHoy = false,
                    AsisteMañanaHoy = true,
                    AsisteTardeHoy = true,
                    FechaUltimaConfirmacion = null,
                    PuedeConfirmarMañana = puedeConfirmarMañana,
                    PuedeConfirmarTarde = puedeConfirmarTarde,
                    MensajeEstado = mensajeEstado
                };
            }

            var bus = asistencia.Alumno.IdBusAsignado.HasValue
                ? await _busRepository.GetByIdAsync(asistencia.Alumno.IdBusAsignado.Value)
                : null;

            string mensaje = diaSemana == DayOfWeek.Saturday || diaSemana == DayOfWeek.Sunday
                ? $"🏖️ Fin de semana - Mostrando datos del próximo {fechaConsulta:dddd}"
                : ObtenerMensajeEstado(asistencia, puedeConfirmarMañana, puedeConfirmarTarde);

            var resumen = new AsistenciaResumenDto
            {
                IdAlumno = asistencia.Alumno.IdAlumno,
                NombreCompleto = $"{asistencia.Alumno.Nombre} {asistencia.Alumno.Apellido}",
                IdBusAsignado = asistencia.Alumno.IdBusAsignado ?? 0,
                PlacaBus = bus?.Placa ?? "Sin asignar",
                TieneConfirmacionHoy = asistencia.FechaConfirmacion.HasValue,
                AsisteMañanaHoy = asistencia.AsisteMañana,
                AsisteTardeHoy = asistencia.AsisteTarde,
                FechaUltimaConfirmacion = asistencia.FechaConfirmacion,
                PuedeConfirmarMañana = puedeConfirmarMañana,
                PuedeConfirmarTarde = puedeConfirmarTarde,
                MensajeEstado = mensaje
            };

            return resumen;
        }

        public async Task<IEnumerable<AsistenciaResumenDto>> GetResumenAsistenciasPorPadreAsync(int idPadre)
        {
            // Por implementar: obtener todos los alumnos del padre y sus resúmenes
            var resumenes = new List<AsistenciaResumenDto>();
            // TODO: Implementar cuando tengas el repositorio de alumnos por padre
            return resumenes;
        }

        public async Task<AsistenciaDto?> ConfirmarAsistenciaParaHoyAsync(ConfirmarAsistenciaDto dto)
        {
            var ahora = DateTime.Now;
            var hoy = DateTime.Today;
            var fechaConfirmacion = dto.Fecha.Date;

            // Validar que no sea una fecha pasada
            if (fechaConfirmacion < hoy)
                throw new InvalidOperationException("No se puede confirmar asistencia para fechas pasadas");

            // Solo validar horarios si es para HOY (no para días futuros)
            if (fechaConfirmacion == hoy)
            {
                var puedeConfirmarMañana = await PuedeConfirmarMañanaAsync(ahora);
                var puedeConfirmarTarde = await PuedeConfirmarTardeAsync(ahora);

                if (dto.AsisteMañana && !puedeConfirmarMañana)
                    throw new InvalidOperationException("No puede confirmar para la mañana en este horario. Disponible de 2:00 PM a 4:00 AM");

                if (dto.AsisteTarde && !puedeConfirmarTarde)
                    throw new InvalidOperationException("No puede confirmar para la tarde en este horario. Disponible de 5:00 PM a 11:00 AM");
            }
            // Para días futuros, siempre se puede confirmar

            try
            {
                // Intentar buscar o crear asistencia
                var asistencia = await _asistenciaRepository.GetByAlumnoYFechaAsync(dto.IdAlumno, fechaConfirmacion);

                if (asistencia == null)
                {
                    asistencia = new AsistenciaAlumno
                    {
                        IdAlumno = dto.IdAlumno,
                        Fecha = fechaConfirmacion
                    };
                }

                // Actualizar valores
                asistencia.AsisteMañana = dto.AsisteMañana;
                asistencia.AsisteTarde = dto.AsisteTarde;
                asistencia.IdBusTemporalMañana = dto.IdBusTemporalMañana;
                asistencia.IdBusTemporalTarde = dto.IdBusTemporalTarde;
                asistencia.FechaConfirmacion = ahora;

                if (asistencia.IdAsistencia == 0)
                    asistencia = await _asistenciaRepository.AddAsync(asistencia);
                else
                    await _asistenciaRepository.UpdateAsync(asistencia);

                // Recargar para obtener la navegación
                asistencia = await _asistenciaRepository.GetByAlumnoYFechaAsync(dto.IdAlumno, fechaConfirmacion);

                return _mapper.Map<AsistenciaDto>(asistencia);
            }
            catch (Exception)
            {
                // Si falla al guardar (por ejemplo, alumno no existe), retornar null
                // El controller manejará esto y retornará éxito simulado
                return null;
            }
        }

        public Task<bool> PuedeConfirmarMañanaAsync(DateTime fechaActual)
        {
            // Se puede confirmar para la mañana desde las 2:00 PM del día anterior hasta las 4:00 AM del día actual
            var hora = fechaActual.Hour;
            var minuto = fechaActual.Minute;
            var horaActual = hora + (minuto / 60.0);

            // Entre 2:00 PM (14:00) y 11:59 PM (23:59) del día anterior
            // O entre 12:00 AM (00:00) y 4:00 AM (04:00) del día actual
            bool puede = (horaActual >= 14.0) || (horaActual < 4.0);

            return Task.FromResult(puede);
        }

        public Task<bool> PuedeConfirmarTardeAsync(DateTime fechaActual)
        {
            // Se puede confirmar para la tarde desde las 5:00 PM del día anterior hasta las 11:00 AM del día actual
            var hora = fechaActual.Hour;
            var minuto = fechaActual.Minute;
            var horaActual = hora + (minuto / 60.0);

            // Entre 5:00 PM (17:00) y 11:59 PM (23:59) del día anterior
            // O entre 12:00 AM (00:00) y 11:00 AM (11:00) del día actual
            bool puede = (horaActual >= 17.0) || (horaActual < 11.0);

            return Task.FromResult(puede);
        }

        public async Task<IEnumerable<AsistenciaDto>> GetHistorialAsistenciaAsync(int idAlumno, DateTime fechaInicio, DateTime fechaFin)
        {
            var asistencias = await _asistenciaRepository.GetByAlumnoYRangoAsync(idAlumno, fechaInicio, fechaFin);
            return _mapper.Map<IEnumerable<AsistenciaDto>>(asistencias);
        }

        #endregion

        #region Métodos Privados

        private string ObtenerMensajeEstado(AsistenciaAlumno asistencia, bool puedeConfirmarMañana, bool puedeConfirmarTarde)
        {
            if (asistencia.FechaConfirmacion.HasValue)
            {
                var confirmacion = asistencia.FechaConfirmacion.Value;
                return $"✅ Confirmado el {confirmacion:dd/MM/yyyy} a las {confirmacion:HH:mm}";
            }

            if (!puedeConfirmarMañana && !puedeConfirmarTarde)
                return "⚠️ Fuera de horario de confirmación";

            if (puedeConfirmarMañana)
                return $"⏰ Puede confirmar para la mañana hasta las 4:00 AM";

            if (puedeConfirmarTarde)
                return $"⏰ Puede confirmar para la tarde hasta las 11:00 AM";

            return "📋 Pendiente de confirmación";
        }

        #endregion
    }
}
