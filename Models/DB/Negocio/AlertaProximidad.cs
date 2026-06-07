using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("AlertasProximidad", Schema = "genesis")]
    public class AlertaProximidad : Auditoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string TipoAlerta { get; set; } // "proximidad", "retraso"

        [Required]
        [StringLength(500)]
        public string Mensaje { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        public int IdBus { get; set; }

        [ForeignKey("IdBus")]
        public Bus Bus { get; set; }

        public int? IdAlumno { get; set; }

        [ForeignKey("IdAlumno")]
        public Alumnos? Alumno { get; set; }

        [Required]
        [StringLength(15)]
        [Column(TypeName = "varchar(15)")]
        public string Estado { get; set; } = "activo"; // "activo", "resuelto"

        public DateTime? FechaResolucion { get; set; }

        // Propiedades adicionales para contexto
        public int? ParadasRestantes { get; set; }

        [StringLength(200)]
        public string? ParadaActual { get; set; }

        [StringLength(200)]
        public string? ParadaDestino { get; set; }

        public bool ConfirmacionPadre { get; set; } = false;

        public DateTime? FechaConfirmacionPadre { get; set; }

        [StringLength(450)]
        public string? IdPadre { get; set; } // FK a AspNetUsers
    }
}