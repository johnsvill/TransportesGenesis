using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("NotificacionProximidad", Schema = "genesis")]
    public class NotificacionProximidad : Auditoria
    {
        [Key]
        public int IdNotificacion { get; set; }

        [ForeignKey("IdParada")]
        public Parada Parada { get; set; }

        [ForeignKey("IdPadre")]
        public Padres Padre { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoNotificacion { get; set; } // "CincoMinutos", "Llegando"

        public bool Enviada { get; set; } = false;

        public DateTime? FechaHoraEnvio { get; set; }
    }
}
