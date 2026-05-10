using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("RegistroRecogida", Schema = "genesis")]
    public class RegistroRecogida : Auditoria
    {
        [Key]
        public int IdRegistro { get; set; }

        [Required]
        public int IdParada { get; set; } // FK a Parada

        [ForeignKey("IdParada")]
        public Parada Parada { get; set; }

        [Required]
        public int IdAlumno { get; set; } // FK a Alumnos

        [ForeignKey("IdAlumno")]
        public Alumnos Alumno { get; set; }

        [Required]
        public DateTime FechaHoraRecogida { get; set; }

        [StringLength(450)]
        public string? ConfirmadoPor { get; set; } // FK a AspNetUsers (Piloto)

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Latitud { get; set; } // Ubicación real donde se recogió

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Longitud { get; set; }

        public bool AlumnoPresente { get; set; } = true; // Si el alumno abordó o no
    }
}
