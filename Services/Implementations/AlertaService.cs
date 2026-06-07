using AutoMapper;
using TransportesGenesis.DTOs.Notificaciones;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio para gestionar alertas de proximidad y retrasos
    /// </summary>
    public class AlertaService : IAlertaService
    {
        private readonly IAlertaProximidadRepository _alertaRepository;
        private readonly IBusRepository _busRepository;
        private readonly INotificacionService _notificacionService;
        private readonly IMapper _mapper;

        public AlertaService(
            IAlertaProximidadRepository alertaRepository,
            IBusRepository busRepository,
            INotificacionService notificacionService,
            IMapper mapper)
        {
            _alertaRepository = alertaRepository;
            _busRepository = busRepository;
            _notificacionService = notificacionService;
            _mapper = mapper;
        }

        public async Task<AlertaProximidadDto> RegistrarAlertaAsync(AlertaProximidadCreateDto dto)
        {
            // Validar que el bus existe
            var busExiste = await _busRepository.ExistsAsync(dto.IdBus);
            if (!busExiste)
            {
                throw new ArgumentException($"El bus con ID {dto.IdBus} no existe.");
            }

            // Crear la entidad AlertaProximidad
            var alerta = _mapper.Map<AlertaProximidad>(dto);
            alerta.FechaHora = DateTime.Now;
            alerta.FechaRegistro = DateTime.Now;
            alerta.Activo = 1;

            // Guardar en base de datos
            var alertaCreada = await _alertaRepository.AddAsync(alerta);

            // Obtener la alerta completa con las relaciones
            var alertaCompleta = await _alertaRepository.GetByIdAsync(alertaCreada.Id);

            // Mapear a DTO y agregar información adicional
            var resultado = _mapper.Map<AlertaProximidadDto>(alertaCompleta);
            await EnriquecerAlertaDto(resultado, alertaCompleta);

            // ✨ NUEVO: Enviar notificación SignalR en tiempo real (Módulo 3)
            try
            {
                await _notificacionService.EnviarAlertaProximidadAsync(
                    resultado.Id, 
                    resultado.IdBus, 
                    resultado.IdAlumno, 
                    resultado.Mensaje, 
                    resultado.TipoAlerta, 
                    resultado.ParadasRestantes);
            }
            catch (Exception ex)
            {
                // No fallar el registro de alerta si falla SignalR
                Console.WriteLine($"⚠️ Error enviando notificación SignalR para alerta {resultado.Id}: {ex.Message}");
            }

            return resultado;
        }

        public async Task<IEnumerable<AlertaProximidadDto>> GetHistorialAlertasPorBusAsync(int idBus, bool incluirResueltas = false)
        {
            var alertas = await _alertaRepository.GetAlertasPorBusAsync(idBus, incluirResueltas);
            var alertasDto = _mapper.Map<IEnumerable<AlertaProximidadDto>>(alertas);

            // Enriquecer cada DTO con información adicional
            foreach (var dto in alertasDto)
            {
                var alerta = alertas.First(a => a.Id == dto.Id);
                await EnriquecerAlertaDto(dto, alerta);
            }

            return alertasDto;
        }

        public async Task<IEnumerable<AlertaProximidadDto>> GetHistorialAlertasPorAlumnoAsync(int idAlumno, bool incluirResueltas = false)
        {
            var alertas = await _alertaRepository.GetAlertasPorAlumnoAsync(idAlumno, incluirResueltas);
            var alertasDto = _mapper.Map<IEnumerable<AlertaProximidadDto>>(alertas);

            // Enriquecer cada DTO con información adicional
            foreach (var dto in alertasDto)
            {
                var alerta = alertas.First(a => a.Id == dto.Id);
                await EnriquecerAlertaDto(dto, alerta);
            }

            return alertasDto;
        }

        public async Task<IEnumerable<AlertaProximidadDto>> GetAlertasActivasPorPadreAsync(string idPadre)
        {
            var alertas = await _alertaRepository.GetAlertasActivasPorPadreAsync(idPadre);
            var alertasDto = _mapper.Map<IEnumerable<AlertaProximidadDto>>(alertas);

            // Enriquecer cada DTO con información adicional
            foreach (var dto in alertasDto)
            {
                var alerta = alertas.First(a => a.Id == dto.Id);
                await EnriquecerAlertaDto(dto, alerta);
            }

            return alertasDto;
        }

        public async Task<bool> MarcarAlertaComoResueltaAsync(int idAlerta)
        {
            var resultado = await _alertaRepository.MarcarComoResueltaAsync(idAlerta);

            // ✨ NUEVO: Notificar resolución via SignalR (Módulo 3)
            if (resultado)
            {
                try
                {
                    var alerta = await _alertaRepository.GetByIdAsync(idAlerta);
                    if (alerta != null)
                    {
                        await _notificacionService.NotificarAlertaResueltaAsync(
                            idAlerta, 
                            alerta.IdBus, 
                            "Alerta resuelta por el sistema");
                    }
                }
                catch (Exception ex)
                {
                    // No fallar la resolución si falla SignalR
                    Console.WriteLine($"⚠️ Error enviando notificación de resolución para alerta {idAlerta}: {ex.Message}");
                }
            }

            return resultado;
        }

        public async Task<bool> ConfirmarRecepcionPadreAsync(int idAlerta)
        {
            var resultado = await _alertaRepository.ConfirmarRecepcionPadreAsync(idAlerta);

            // ✨ NUEVO: Notificar confirmación via SignalR (Módulo 3)
            if (resultado)
            {
                try
                {
                    var alerta = await _alertaRepository.GetByIdAsync(idAlerta);
                    if (alerta != null && !string.IsNullOrEmpty(alerta.IdPadre))
                    {
                        await _notificacionService.NotificarConfirmacionAlertaAsync(idAlerta, alerta.IdPadre);
                    }
                }
                catch (Exception ex)
                {
                    // No fallar la confirmación si falla SignalR
                    Console.WriteLine($"⚠️ Error enviando notificación de confirmación para alerta {idAlerta}: {ex.Message}");
                }
            }

            return resultado;
        }

        public async Task<AlertaProximidadDto?> GetAlertaPorIdAsync(int idAlerta)
        {
            var alerta = await _alertaRepository.GetByIdAsync(idAlerta);
            if (alerta == null) return null;

            var dto = _mapper.Map<AlertaProximidadDto>(alerta);
            await EnriquecerAlertaDto(dto, alerta);

            return dto;
        }

        public async Task<IEnumerable<AlertaProximidadDto>> GetAlertasActivasPorTipoAsync(string tipoAlerta)
        {
            var alertas = await _alertaRepository.GetAlertasPorTipoYEstadoAsync(tipoAlerta, "activo");
            var alertasDto = _mapper.Map<IEnumerable<AlertaProximidadDto>>(alertas);

            // Enriquecer cada DTO con información adicional
            foreach (var dto in alertasDto)
            {
                var alerta = alertas.First(a => a.Id == dto.Id);
                await EnriquecerAlertaDto(dto, alerta);
            }

            return alertasDto;
        }

        public async Task<int> LimpiarAlertasAntiguasAsync(int diasAntiguedad = 30)
        {
            var fechaLimite = DateTime.Now.AddDays(-diasAntiguedad);
            var alertasAntiguas = await _alertaRepository.GetAlertasAntiguasResueltasAsync(fechaLimite);

            if (!alertasAntiguas.Any()) return 0;

            var ids = alertasAntiguas.Select(a => a.Id);
            return await _alertaRepository.DeleteMultipleAsync(ids);
        }

        public async Task<AlertaProximidadDto?> ActualizarAlertaAsync(AlertaProximidadUpdateDto dto)
        {
            var alerta = await _alertaRepository.GetByIdAsync(dto.Id);
            if (alerta == null) return null;

            // Actualizar campos
            alerta.Estado = dto.Estado;
            if (dto.FechaResolucion.HasValue)
            {
                alerta.FechaResolucion = dto.FechaResolucion.Value;
            }

            var alertaActualizada = await _alertaRepository.UpdateAsync(alerta);
            var resultado = _mapper.Map<AlertaProximidadDto>(alertaActualizada);
            await EnriquecerAlertaDto(resultado, alertaActualizada);

            return resultado;
        }

        /// <summary>
        /// ✨ MÓDULO 6: Genera datos de ejemplo para pruebas del sistema de alertas
        /// </summary>
        public async Task<bool> GenerarDatosDeEjemploAsync()
        {
            try
            {
                Console.WriteLine("🔧 Generando datos de ejemplo para el sistema de alertas...");

                // Obtener algunos buses existentes para usar en los ejemplos
                var busesExistentes = await _busRepository.GetAllAsync();

                if (!busesExistentes.Any())
                {
                    Console.WriteLine("❌ No hay buses registrados. No se pueden generar datos de ejemplo.");
                    return false;
                }

                var listaBuses = busesExistentes.ToList();
                var random = new Random();

                // Lista de datos de ejemplo variados
                var alertasEjemplo = new List<AlertaProximidad>();

                // 🔔 Alertas de proximidad activas (recientes)
                for (int i = 1; i <= 5; i++)
                {
                    var busAleatorio = listaBuses[random.Next(listaBuses.Count)];
                    alertasEjemplo.Add(new AlertaProximidad
                    {
                        TipoAlerta = "proximidad",
                        Mensaje = $"Bus {busAleatorio.Placa} se acerca a la parada del estudiante en 2-3 minutos",
                        Estado = "activo",
                        FechaHora = DateTime.Now.AddMinutes(-random.Next(5, 30)),
                        FechaRegistro = DateTime.Now.AddMinutes(-random.Next(5, 30)),
                        IdBus = busAleatorio.IdBus,
                        IdAlumno = random.Next(1, 20), // Asumiendo que hay alumnos con IDs del 1-20
                        ParadaActual = $"Parada Centro {i}",
                        ParadaDestino = $"Colegio Principal",
                        ParadasRestantes = random.Next(1, 4),
                        Activo = 1
                    });
                }

                // ⏰ Alertas de retraso
                for (int i = 1; i <= 3; i++)
                {
                    var busAleatorio = listaBuses[random.Next(listaBuses.Count)];
                    var minutosRetraso = random.Next(5, 25);
                    alertasEjemplo.Add(new AlertaProximidad
                    {
                        TipoAlerta = "retraso",
                        Mensaje = $"Bus {busAleatorio.Placa} presenta retraso de {minutosRetraso} minutos debido a tráfico",
                        Estado = random.Next(1, 4) <= 2 ? "activo" : "resuelto", // 50% activas, 50% resueltas
                        FechaHora = DateTime.Now.AddHours(-random.Next(1, 6)),
                        FechaRegistro = DateTime.Now.AddHours(-random.Next(1, 6)),
                        IdBus = busAleatorio.IdBus,
                        IdAlumno = random.Next(1, 20),
                        ParadaActual = $"Ruta Principal {i}",
                        ParadaDestino = "Colegio Principal",
                        ParadasRestantes = random.Next(2, 8),
                        Activo = 1,
                        FechaResolucion = random.Next(1, 4) > 2 ? DateTime.Now.AddMinutes(-random.Next(10, 120)) : null
                    });
                }

                // 📍 Alertas adicionales de proximidad
                for (int i = 1; i <= 2; i++)
                {
                    var busAleatorio = listaBuses[random.Next(listaBuses.Count)];
                    alertasEjemplo.Add(new AlertaProximidad
                    {
                        TipoAlerta = "proximidad", // ✅ Valor permitido
                        Mensaje = $"Bus {busAleatorio.Placa} se encuentra acercándose a zona de alta densidad de tráfico",
                        Estado = "activo",
                        FechaHora = DateTime.Now.AddMinutes(-random.Next(15, 45)),
                        FechaRegistro = DateTime.Now.AddMinutes(-random.Next(15, 45)),
                        IdBus = busAleatorio.IdBus,
                        IdAlumno = random.Next(1, 20),
                        ParadaActual = $"Zona Centro {i}",
                        ParadaDestino = "Colegio Principal",
                        ParadasRestantes = random.Next(3, 6),
                        Activo = 1
                    });
                }

                // 📊 Alertas históricas resueltas (para demostrar historial)
                for (int i = 1; i <= 8; i++)
                {
                    var busAleatorio = listaBuses[random.Next(listaBuses.Count)];
                    var diasAtras = random.Next(1, 15);
                    var tiposAlertas = new[] { "proximidad", "retraso" }; // ✅ Solo valores permitidos
                    var tipoAleatorio = tiposAlertas[random.Next(tiposAlertas.Length)];

                    alertasEjemplo.Add(new AlertaProximidad
                    {
                        TipoAlerta = tipoAleatorio,
                        Mensaje = $"Alerta histórica {tipoAleatorio}: Bus {busAleatorio.Placa} - Evento resuelto",
                        Estado = "resuelto",
                        FechaHora = DateTime.Now.AddDays(-diasAtras).AddHours(-random.Next(1, 10)),
                        FechaRegistro = DateTime.Now.AddDays(-diasAtras).AddHours(-random.Next(1, 10)),
                        FechaResolucion = DateTime.Now.AddDays(-diasAtras).AddMinutes(-random.Next(30, 300)),
                        IdBus = busAleatorio.IdBus,
                        IdAlumno = random.Next(1, 20),
                        ParadaActual = $"Parada Histórica {i}",
                        ParadaDestino = "Destino Resuelto",
                        ParadasRestantes = 0,
                        Activo = 1
                    });
                }

                // Insertar todas las alertas de ejemplo en la base de datos
                foreach (var alerta in alertasEjemplo)
                {
                    await _alertaRepository.AddAsync(alerta);
                }

                Console.WriteLine($"✅ Generados {alertasEjemplo.Count} registros de ejemplo:");
                Console.WriteLine($"   • {alertasEjemplo.Count(a => a.TipoAlerta == "proximidad")} alertas de proximidad");
                Console.WriteLine($"   • {alertasEjemplo.Count(a => a.TipoAlerta == "retraso")} alertas de retraso");
                Console.WriteLine($"   • {alertasEjemplo.Count(a => a.Estado == "activo")} alertas activas");
                Console.WriteLine($"   • {alertasEjemplo.Count(a => a.Estado == "resuelto")} alertas resueltas");
                Console.WriteLine("🎯 Datos listos para pruebas del dashboard de alertas");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error generando datos de ejemplo: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Método privado para enriquecer el DTO con información adicional
        /// </summary>
        private async Task EnriquecerAlertaDto(AlertaProximidadDto dto, AlertaProximidad alerta)
        {
            // Información del bus
            if (alerta.Bus != null)
            {
                dto.NombreBus = $"{alerta.Bus.Placa} - {alerta.Bus.Modelo}";
            }

            // Información del alumno
            if (alerta.Alumno != null)
            {
                dto.NombreAlumno = $"{alerta.Alumno.Nombre} {alerta.Alumno.Apellido}";

                // Si el alumno tiene padre asociado, obtener el nombre del padre
                if (alerta.Alumno.Padres != null)
                {
                    dto.NombrePadre = $"{alerta.Alumno.Padres.Nombre} {alerta.Alumno.Padres.Apellido}";
                }
            }

            // Asignar información de paradas y paradas restantes
            dto.NombreParadaActual = alerta.ParadaActual;
            dto.NombreParadaDestino = alerta.ParadaDestino;
            dto.ParadasRestantes = alerta.ParadasRestantes;

            await Task.CompletedTask; // Para mantener la signatura async
        }
    }
}