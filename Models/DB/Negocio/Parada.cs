using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("Paradas", Schema = "genesis")]
    public class Parada : Auditoria
    {
        [Key]
        public int IdParada { get; set; }

        [ForeignKey("IdRuta")]
        public Ruta Ruta { get; set; }

        [ForeignKey("IdAlumno")]
        public Alumnos Alumno { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 7)")]
        public decimal Latitud { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 7)")]
        public decimal Longitud { get; set; }

        [StringLength(250)]
        public string? Direccion { get; set; }

        public int Orden { get; set; } // Orden de parada en la ruta

        public TimeSpan? HoraEstimada { get; set; }

        public bool Completada { get; set; } = false;

        // Navegación
        public List<RegistroRecogida> RegistrosRecogidaLink { get; set; }
        public List<NotificacionProximidad> NotificacionesLink { get; set; }
    }
}
