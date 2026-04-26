using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("NotificacionRetraso", Schema = "genesis")]
    public class NotificacionRetraso : Auditoria
    {
        [Key]
        public int IdNotificacion { get; set; }

        [ForeignKey("IdRuta")]
        public Ruta Ruta { get; set; }

        [Range(0, 999)]
        public int TiempoRetrasoMinutos { get; set; }

        [Required]
        [StringLength(100)]
        public string Motivo { get; set; } // "Trafico", "Accidente", "Desviacion"

        [Required]
        public DateTime FechaHora { get; set; }

        public bool NotificadoAPadres { get; set; } = false;

        public DateTime? FechaNotificacionPadres { get; set; }
    }
}
