using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("Alertas", Schema = "genesis")]
    public class Alerta : Auditoria
    {
        [Key]
        public int IdAlerta { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoAlerta { get; set; } // "Retraso", "Desvio", "Emergencia", etc.

        [Required]
        [StringLength(500)]
        public string Mensaje { get; set; }

        [StringLength(450)]
        public string? IdRemitente { get; set; } // FK a AspNetUsers (Admin)

        [StringLength(450)]
        public string? IdDestinatario { get; set; } // FK a AspNetUsers (Padre/Piloto)

        [Required]
        public DateTime FechaEnvio { get; set; }

        public bool Leida { get; set; } = false;

        public DateTime? FechaLectura { get; set; }
    }
}
