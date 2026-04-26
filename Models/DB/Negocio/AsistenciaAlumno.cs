using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("AsistenciaAlumno", Schema = "genesis")]
    public class AsistenciaAlumno : Auditoria
    {
        [Key]
        public int IdAsistencia { get; set; }

        [Required]
        public int IdAlumno { get; set; }

        [ForeignKey("IdAlumno")]
        public Alumnos Alumno { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public bool AsisteMañana { get; set; } = true;

        public bool AsisteTarde { get; set; } = true;

        public DateTime? FechaConfirmacion { get; set; }

        public int? IdBusTemporalMañana { get; set; } // Para traslados temporales

        public int? IdBusTemporalTarde { get; set; } // Para traslados temporales
    }
}
