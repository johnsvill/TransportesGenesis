using TransportesGenesis.DTOs.Notificaciones;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Tests
{
    /// <summary>
    /// Clase de prueba para verificar la funcionalidad del AlertaService
    /// </summary>
    public class AlertaProximidadTest
    {
        private readonly IAlertaService _alertaService;

        public AlertaProximidadTest(IAlertaService alertaService)
        {
            _alertaService = alertaService;
        }

        /// <summary>
        /// Método de prueba para crear una alerta de proximidad
        /// </summary>
        public async Task<AlertaProximidadDto> CrearAlertaPruebaAsync()
        {
            var alertaDto = new AlertaProximidadCreateDto
            {
                TipoAlerta = "proximidad",
                Mensaje = "El bus está a 2 paradas de la parada asignada",
                IdBus = 1, // Asumiendo que existe un bus con ID 1
                IdAlumno = 1, // Asumiendo que existe un alumno con ID 1
                Estado = "activo"
            };

            var alerta = await _alertaService.RegistrarAlertaAsync(alertaDto);
            return alerta;
        }

        /// <summary>
        /// Método para obtener alertas activas por bus
        /// </summary>
        public async Task<IEnumerable<AlertaProximidadDto>> ObtenerAlertasActivasPorBusAsync(int idBus)
        {
            return await _alertaService.GetHistorialAlertasPorBusAsync(idBus, incluirResueltas: false);
        }

        /// <summary>
        /// Método para confirmar recepción de alerta por parte del padre
        /// </summary>
        public async Task<bool> ConfirmarAlertaPadreAsync(int idAlerta)
        {
            return await _alertaService.ConfirmarRecepcionPadreAsync(idAlerta);
        }

        /// <summary>
        /// Método para resolver una alerta
        /// </summary>
        public async Task<bool> ResolverAlertaAsync(int idAlerta)
        {
            return await _alertaService.MarcarAlertaComoResueltaAsync(idAlerta);
        }

        /// <summary>
        /// Método de prueba completo que simula el flujo completo de una alerta
        /// </summary>
        public async Task<string> PruebaFlujoCompletoAsync()
        {
            try
            {
                // 1. Crear alerta
                var alertaCreada = await CrearAlertaPruebaAsync();
                Console.WriteLine($"✅ Alerta creada con ID: {alertaCreada.Id}");

                // 2. Obtener alertas activas
                var alertasActivas = await ObtenerAlertasActivasPorBusAsync(alertaCreada.IdBus);
                Console.WriteLine($"✅ Alertas activas encontradas: {alertasActivas.Count()}");

                // 3. Confirmar recepción por padre
                var confirmada = await ConfirmarAlertaPadreAsync(alertaCreada.Id);
                Console.WriteLine($"✅ Confirmación de padre: {confirmada}");

                // 4. Resolver alerta
                var resuelta = await ResolverAlertaAsync(alertaCreada.Id);
                Console.WriteLine($"✅ Alerta resuelta: {resuelta}");

                return $"Prueba completada exitosamente. ID de alerta: {alertaCreada.Id}";
            }
            catch (Exception ex)
            {
                return $"❌ Error en la prueba: {ex.Message}";
            }
        }
    }
}