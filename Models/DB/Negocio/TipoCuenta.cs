using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("TipoCuenta", Schema = "genesis")]
    public class TipoCuenta : Auditoria
    {
        [Key]
        public int IdTipoCuenta { get; set; }

        public string Nombre { get; set; }

        [ForeignKey("IdBanco")]
        public Banco Bancos { get; set; }

        [ForeignKey("IdPadre")]
        public Padres Padres { get; set; }

        public List<Pago> PagosLink { get; set; }
    }
}
