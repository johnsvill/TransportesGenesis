using System.ComponentModel.DataAnnotations;

namespace TransportesGenesis.DTOs.Asistencia
{
    public class AsistenciaDto
    {
        public int IdAsistencia { get; set; }
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; }
        public DateTime Fecha { get; set; }
        public bool AsisteMañana { get; set; }
        public bool AsisteTarde { get; set; }
        public DateTime? FechaConfirmacion { get; set; }
        public int? IdBusTemporalMañana { get; set; }
        public int? IdBusTemporalTarde { get; set; }
    }

    public class ConfirmarAsistenciaDto
    {
        [Required(ErrorMessage = "El ID del alumno es requerido")]
        public int IdAlumno { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe indicar si asiste en la mañana")]
        public bool AsisteMañana { get; set; }

        [Required(ErrorMessage = "Debe indicar si asiste en la tarde")]
        public bool AsisteTarde { get; set; }

        public int? IdBusTemporalMañana { get; set; }
        public int? IdBusTemporalTarde { get; set; }
    }

    public class AsistenciaResumenDto
    {
        public int IdAlumno { get; set; }
        public string NombreCompleto { get; set; }
        public int IdBusAsignado { get; set; }
        public string PlacaBus { get; set; }
        public bool TieneConfirmacionHoy { get; set; }
        public bool AsisteMañanaHoy { get; set; }
        public bool AsisteTardeHoy { get; set; }
        public DateTime? FechaUltimaConfirmacion { get; set; }
        public bool PuedeConfirmarMañana { get; set; }
        public bool PuedeConfirmarTarde { get; set; }
        public string MensajeEstado { get; set; }
    }
}
