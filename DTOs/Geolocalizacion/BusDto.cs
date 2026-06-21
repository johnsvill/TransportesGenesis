namespace TransportesGenesis.DTOs.Geolocalizacion
{
    public class BusDto
    {
        public int IdBus { get; set; }
        public string Placa { get; set; }
        public string? Modelo { get; set; }
        public int Capacidad { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Datos adicionales para vistas
        public int RutasAsignadas { get; set; }
        /// <summary>Rutas en recorrido ahora (EsActiva + GPS reciente).</summary>
        public int RutasActivas { get; set; }
        public string? PilotoAsignado { get; set; }
    }

    public class BusCreateDto
    {
        public string Placa { get; set; }
        public string? Modelo { get; set; }
        public int Capacidad { get; set; }
    }

    public class BusUpdateDto
    {
        public int IdBus { get; set; }
        public string Placa { get; set; }
        public string? Modelo { get; set; }
        public int Capacidad { get; set; }
        public bool Estado { get; set; }
    }
}
