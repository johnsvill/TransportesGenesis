using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("AsignacionPilotoBus", Schema = "genesis")]
    public class AsignacionPilotoBus : Auditoria
    {
        [Key]
        public int IdAsignacion { get; set; }

        [Required]
        [StringLength(450)]
        public string IdUsuarioPiloto { get; set; } // FK a AspNetUsers

        [Required]
        public int IdBus { get; set; }

        [ForeignKey("IdBus")]
        public Bus Bus { get; set; }

        [Required]
        public DateTime FechaAsignacion { get; set; }

        public DateTime? FechaFinAsignacion { get; set; }

        public bool EsActual { get; set; } = true;
    }
}
