using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("SolicitudTraslado", Schema = "genesis")]
    public class SolicitudTraslado : Auditoria
    {
        [Key]
        public int IdSolicitud { get; set; }

        [Required]
        public int IdAlumno { get; set; }

        [ForeignKey("IdAlumno")]
        public Alumnos Alumno { get; set; }

        [Required]
        public int IdBusOrigen { get; set; }

        [ForeignKey("IdBusOrigen")]
        public Bus BusOrigen { get; set; }

        public int? IdBusDestino { get; set; }

        [ForeignKey("IdBusDestino")]
        public Bus? BusDestino { get; set; }

        [Required]
        public DateTime FechaTraslado { get; set; }

        [Required]
        [StringLength(10)]
        public string Turno { get; set; } = "Ambos"; // "Mañana", "Tarde", "Ambos"

        [StringLength(250)]
        public string? Motivo { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Aprobado, Rechazado

        [StringLength(450)]
        public string? AprobadoPor { get; set; } // FK a AspNetUsers (Admin)

        public DateTime? FechaRespuesta { get; set; }

        [StringLength(250)]
        public string? ComentarioAdmin { get; set; }
    }
}
