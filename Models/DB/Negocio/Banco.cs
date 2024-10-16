using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("Bancos", Schema = "genesis")]
    public class Banco : Auditoria
    {
        [Key]
        public int IdBanco { get; set; }

        public string Nombre { get; set; }

        public List<TipoCuenta> TipoCuentasLink { get; set; }   
    }
}
