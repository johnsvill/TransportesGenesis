namespace TransportesGenesis.DTOs.Notificaciones
{
    /// <summary>
    /// DTO para notificación de bus cercano
    /// </summary>
    public class BusCercaDto
    {
        public int IdBus { get; set; }
        public int IdParada { get; set; }
        public decimal DistanciaKm { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }

    /// <summary>
    /// DTO para notificación de parada completada
    /// </summary>
    public class ParadaCompletadaDto
    {
        public int IdAlumno { get; set; }
        public int IdParada { get; set; }
        public string NombreAlumno { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }

    /// <summary>
    /// DTO para notificación de retraso
    /// </summary>
    public class RetrasoDto
    {
        public int IdBus { get; set; }
        public int MinutosRetraso { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }

    /// <summary>
    /// DTO para mensaje broadcast
    /// </summary>
    public class BroadcastDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string Tipo { get; set; } = "info"; // info, warning, error, success
        public DateTime FechaHora { get; set; }
    }

    /// <summary>
    /// DTO para actualización de ubicación de bus
    /// </summary>
    public class UbicacionBusDto
    {
        public int IdBus { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime FechaHora { get; set; }
    }

    /// <summary>
    /// DTO para mensaje a un bus específico
    /// </summary>
    public class MensajeBusDto
    {
        public int IdBus { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }
}
