using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("MontosPadres", Schema = "genesis")]
    public class MontoPadre : Auditoria
    {
        [Key]
        public int IdMontoPadre { get; set; }
        public string UsuarioId { get; set; }
        public decimal MontoAsignado { get; set; }
    }
}
