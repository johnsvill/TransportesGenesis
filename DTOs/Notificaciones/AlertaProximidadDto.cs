namespace TransportesGenesis.DTOs.Notificaciones
{
    public class AlertaProximidadDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Tipo de alerta: "proximidad", "retraso"
        /// </summary>
        public string TipoAlerta { get; set; }

        /// <summary>
        /// Mensaje descriptivo de la alerta
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Fecha y hora de generación de la alerta
        /// </summary>
        public DateTime FechaHora { get; set; }

        /// <summary>
        /// ID del bus que genera la alerta
        /// </summary>
        public int IdBus { get; set; }

        /// <summary>
        /// ID del alumno (opcional) al que aplica la alerta
        /// </summary>
        public int? IdAlumno { get; set; }

        /// <summary>
        /// Estado de la alerta: "activo", "resuelto"
        /// </summary>
        public string Estado { get; set; }

        // Propiedades adicionales para el frontend
        public string? NombreBus { get; set; }
        public string? NombreAlumno { get; set; }
        public string? NombrePadre { get; set; }
        public int? ParadasRestantes { get; set; }
        public string? NombreParadaActual { get; set; }
        public string? NombreParadaDestino { get; set; }
    }

    public class AlertaProximidadCreateDto
    {
        public string TipoAlerta { get; set; }
        public string Mensaje { get; set; }
        public int IdBus { get; set; }
        public int? IdAlumno { get; set; }
        public string Estado { get; set; } = "activo";
    }

    public class AlertaProximidadUpdateDto
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaResolucion { get; set; }
    }
}