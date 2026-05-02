using Microsoft.AspNetCore.Mvc;
using TransportesGenesis.DTOs.Geolocalizacion;

namespace TransportesGenesis.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeolocalizacionController : ControllerBase
    {
        // Datos simulados de buses en zonas reales de Guatemala
        private readonly List<BusGeolocalizacionDto> _busesData = new()
        {
            new BusGeolocalizacionDto
            {
                IdBus = 1,
                Placa = "P-001CX",
                Conductor = "Juan Pérez Morales",
                RutaNombre = "Zona 1 - Colegio La Asunción",
                RutaDescripcion = "Centro Histórico → Colegio La Asunción (Zona 14)",
                Latitud = 14.6418,
                Longitud = -90.5133,
                Velocidad = 25,
                Direccion = 45, // Grados de dirección
                Estado = "En Movimiento",
                UltimaActualizacion = DateTime.Now,
                Paradas = new List<ParadaGeolocalizacionDto>
                {
                    new() { Nombre = "Plaza de la Constitución", Latitud = 14.6418, Longitud = -90.5133, Completada = true },
                    new() { Nombre = "6ta Avenida Zona 1", Latitud = 14.6398, Longitud = -90.5118, Completada = true },
                    new() { Nombre = "Torre del Reformador", Latitud = 14.6025, Longitud = -90.5200, Completada = true },
                    new() { Nombre = "Zona Rosa (Zona 10)", Latitud = 14.5955, Longitud = -90.5064, Completada = false },
                    new() { Nombre = "Colegio La Asunción", Latitud = 14.5889, Longitud = -90.4934, Completada = false }
                },
                CapacidadTotal = 40,
                EstudiantesAbordo = 12
            },
            new BusGeolocalizacionDto
            {
                IdBus = 2,
                Placa = "P-002CX",
                Conductor = "María Elena González",
                RutaNombre = "Zona 5 - Liceo Javier",
                RutaDescripcion = "Zona 5 → Liceo Javier (Zona 10)",
                Latitud = 14.6156,
                Longitud = -90.5342,
                Velocidad = 0,
                Direccion = 0,
                Estado = "Detenido",
                UltimaActualizacion = DateTime.Now.AddMinutes(-3),
                Paradas = new List<ParadaGeolocalizacionDto>
                {
                    new() { Nombre = "Terminal Zona 5", Latitud = 14.6156, Longitud = -90.5342, Completada = true },
                    new() { Nombre = "Roosevelt y Montúfar", Latitud = 14.6089, Longitud = -90.5278, Completada = true },
                    new() { Nombre = "Plaza España", Latitud = 14.6067, Longitud = -90.5189, Completada = true },
                    new() { Nombre = "Zona 9 Centro", Latitud = 14.6012, Longitud = -90.5123, Completada = false },
                    new() { Nombre = "Liceo Javier", Latitud = 14.5978, Longitud = -90.5089, Completada = false }
                },
                CapacidadTotal = 35,
                EstudiantesAbordo = 8
            },
            new BusGeolocalizacionDto
            {
                IdBus = 3,
                Placa = "P-003CX",
                Conductor = "Carlos Rodríguez Paz",
                RutaNombre = "Cayalá - Colegio Yulimai",
                RutaDescripcion = "Cayalá (Zona 16) → Colegio Yulimai PC (Zona 13)",
                Latitud = 14.6187,
                Longitud = -90.4756,
                Velocidad = 32,
                Direccion = 180,
                Estado = "En Movimiento",
                UltimaActualizacion = DateTime.Now.AddSeconds(-15),
                Paradas = new List<ParadaGeolocalizacionDto>
                {
                    new() { Nombre = "Paseo Cayalá", Latitud = 14.6234, Longitud = -90.4723, Completada = true },
                    new() { Nombre = "Oakland Mall", Latitud = 14.6187, Longitud = -90.4756, Completada = true },
                    new() { Nombre = "Carretera a El Salvador", Latitud = 14.6145, Longitud = -90.4834, Completada = true },
                    new() { Nombre = "Bulevar Vista Hermosa", Latitud = 14.6089, Longitud = -90.4889, Completada = false },
                    new() { Nombre = "Colegio Yulimai PC", Latitud = 14.6034, Longitud = -90.4923, Completada = false }
                },
                CapacidadTotal = 45,
                EstudiantesAbordo = 23
            },
            new BusGeolocalizacionDto
            {
                IdBus = 4,
                Placa = "P-004CX",
                Conductor = "Ana Sofía Morales",
                RutaNombre = "Zona 4 - Colegio Americano",
                RutaDescripcion = "Zona 4 → Colegio Americano de Guatemala",
                Latitud = 14.6267,
                Longitud = -90.5445,
                Velocidad = 18,
                Direccion = 90,
                Estado = "En Movimiento",
                UltimaActualizacion = DateTime.Now.AddSeconds(-8),
                Paradas = new List<ParadaGeolocalizacionDto>
                {
                    new() { Nombre = "Centro Comercial Zona 4", Latitud = 14.6289, Longitud = -90.5467, Completada = true },
                    new() { Nombre = "Hospital Roosevelt", Latitud = 14.6267, Longitud = -90.5445, Completada = true },
                    new() { Nombre = "Avenida Petapa", Latitud = 14.6234, Longitud = -90.5389, Completada = false },
                    new() { Nombre = "Bulevar Liberación", Latitud = 14.6198, Longitud = -90.5323, Completada = false },
                    new() { Nombre = "Colegio Americano", Latitud = 14.6156, Longitud = -90.5267, Completada = false }
                },
                CapacidadTotal = 38,
                EstudiantesAbordo = 15
            },
            new BusGeolocalizacionDto
            {
                IdBus = 5,
                Placa = "P-005CX",
                Conductor = "Roberto Méndez Silva",
                RutaNombre = "Mixco - Liceo Guatemala",
                RutaDescripcion = "Mixco → Liceo Guatemala (Zona 2)",
                Latitud = 14.6378,
                Longitud = -90.6067,
                Velocidad = 0,
                Direccion = 0,
                Estado = "Sin Señal",
                UltimaActualizacion = DateTime.Now.AddMinutes(-12),
                Paradas = new List<ParadaGeolocalizacionDto>
                {
                    new() { Nombre = "Centro Mixco", Latitud = 14.6378, Longitud = -90.6067, Completada = true },
                    new() { Nombre = "San Cristóbal", Latitud = 14.6345, Longitud = -90.5934, Completada = true },
                    new() { Nombre = "Colonia El Milagro", Latitud = 14.6312, Longitud = -90.5812, Completada = false },
                    new() { Nombre = "Terminal Zona 2", Latitud = 14.6289, Longitud = -90.5634, Completada = false },
                    new() { Nombre = "Liceo Guatemala", Latitud = 14.6267, Longitud = -90.5456, Completada = false }
                },
                CapacidadTotal = 42,
                EstudiantesAbordo = 0
            }
        };

        [HttpGet("buses")]
        public ActionResult<IEnumerable<BusGeolocalizacionDto>> GetBusesActivos()
        {
            try
            {
                // Simular movimiento en tiempo real
                ActualizarPosicionesBuses();

                return Ok(_busesData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener datos de buses", error = ex.Message });
            }
        }

        [HttpGet("buses/{id}")]
        public ActionResult<BusGeolocalizacionDto> GetBus(int id)
        {
            try
            {
                var bus = _busesData.FirstOrDefault(b => b.IdBus == id);
                if (bus == null)
                {
                    return NotFound(new { message = $"Bus con ID {id} no encontrado" });
                }

                return Ok(bus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener datos del bus", error = ex.Message });
            }
        }

        [HttpGet("estadisticas")]
        public ActionResult<object> GetEstadisticas()
        {
            try
            {
                var stats = new
                {
                    TotalBuses = _busesData.Count,
                    BusesActivos = _busesData.Count(b => b.Estado == "En Movimiento"),
                    BusesDetenidos = _busesData.Count(b => b.Estado == "Detenido"),
                    BusesSinSenal = _busesData.Count(b => b.Estado == "Sin Señal"),
                    VelocidadPromedio = _busesData.Where(b => b.Estado == "En Movimiento").Average(b => b.Velocidad),
                    EstudiantesTotales = _busesData.Sum(b => b.EstudiantesAbordo),
                    CapacidadTotal = _busesData.Sum(b => b.CapacidadTotal),
                    UltimaActualizacion = DateTime.Now
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener estadísticas", error = ex.Message });
            }
        }

        private void ActualizarPosicionesBuses()
        {
            var random = new Random();

            foreach (var bus in _busesData)
            {
                if (bus.Estado == "En Movimiento")
                {
                    // Simular movimiento a lo largo de la ruta
                    var siguienteParada = bus.Paradas.FirstOrDefault(p => !p.Completada);
                    if (siguienteParada != null)
                    {
                        // Mover hacia la siguiente parada
                        var deltaLat = (siguienteParada.Latitud - bus.Latitud) * 0.01; // 1% del camino
                        var deltaLng = (siguienteParada.Longitud - bus.Longitud) * 0.01;

                        bus.Latitud += deltaLat + (random.NextDouble() - 0.5) * 0.0001; // Pequeña variación aleatoria
                        bus.Longitud += deltaLng + (random.NextDouble() - 0.5) * 0.0001;

                        // Calcular dirección de movimiento
                        var angle = Math.Atan2(deltaLng, deltaLat) * (180 / Math.PI);
                        bus.Direccion = (int)(angle >= 0 ? angle : angle + 360);

                        // Verificar si llegó a la parada (proximidad de 50 metros aproximadamente)
                        var distancia = CalcularDistancia(bus.Latitud, bus.Longitud, siguienteParada.Latitud, siguienteParada.Longitud);
                        if (distancia < 0.0005) // Aproximadamente 50 metros
                        {
                            siguienteParada.Completada = true;
                            // Simular recogida de estudiantes
                            bus.EstudiantesAbordo += random.Next(1, 4);
                            if (bus.EstudiantesAbordo > bus.CapacidadTotal)
                            {
                                bus.EstudiantesAbordo = bus.CapacidadTotal;
                            }
                        }
                    }

                    // Actualizar velocidad aleatoriamente
                    if (random.Next(1, 100) > 85) // 15% de probabilidad
                    {
                        bus.Velocidad = random.Next(15, 35);
                    }
                }
                else if (bus.Estado == "Detenido")
                {
                    // Posibilidad de volver a moverse
                    if (random.Next(1, 100) > 95) // 5% de probabilidad
                    {
                        bus.Estado = "En Movimiento";
                        bus.Velocidad = random.Next(15, 25);
                    }
                }

                bus.UltimaActualizacion = DateTime.Now;
            }
        }

        private static double CalcularDistancia(double lat1, double lng1, double lat2, double lng2)
        {
            return Math.Sqrt(Math.Pow(lat2 - lat1, 2) + Math.Pow(lng2 - lng1, 2));
        }
    }
}