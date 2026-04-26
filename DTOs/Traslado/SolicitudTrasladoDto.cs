using System.ComponentModel.DataAnnotations;

namespace TransportesGenesis.DTOs.Traslado
{
    public class SolicitudTrasladoDto
    {
        public int IdSolicitud { get; set; }
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; }
        public int IdBusOrigen { get; set; }
        public string PlacaBusOrigen { get; set; }
        public int? IdBusDestino { get; set; }
        public string? PlacaBusDestino { get; set; }
        public DateTime FechaTraslado { get; set; }
        public string Turno { get; set; }
        public string? Motivo { get; set; }
        public string Estado { get; set; }
        public string? AprobadoPor { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string? ComentarioAdmin { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class CrearSolicitudTrasladoDto
    {
        [Required(ErrorMessage = "El ID del alumno es requerido")]
        public int IdAlumno { get; set; }

        [Required(ErrorMessage = "La fecha de traslado es requerida")]
        public DateTime FechaTraslado { get; set; }

        [Required(ErrorMessage = "El turno es requerido")]
        [RegularExpression("^(Mañana|Tarde|Ambos)$", ErrorMessage = "El turno debe ser Mañana, Tarde o Ambos")]
        public string Turno { get; set; } = "Ambos";

        public int? IdBusDestino { get; set; }

        [StringLength(250, ErrorMessage = "El motivo no puede exceder 250 caracteres")]
        public string? Motivo { get; set; }
    }

    public class ResponderSolicitudTrasladoDto
    {
        [Required(ErrorMessage = "El ID de la solicitud es requerido")]
        public int IdSolicitud { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [RegularExpression("^(Aprobado|Rechazado)$", ErrorMessage = "El estado debe ser Aprobado o Rechazado")]
        public string Estado { get; set; }

        public int? IdBusDestino { get; set; }

        [StringLength(250, ErrorMessage = "El comentario no puede exceder 250 caracteres")]
        public string? ComentarioAdmin { get; set; }
    }

    public class BusDisponibleDto
    {
        public int IdBus { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int Capacidad { get; set; }
        public int AsientosDisponibles { get; set; }
    }
}
