using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
   
    [Table("CuentasUsuarios", Schema = "genesis")]
    public class CuentaUsuario : Auditoria
    {
        [Key]
        public int IdCuentaUsuario { get; set; }
        public string UsuarioId { get; set; }
        public int IdBanco { get; set; }
        public string NumeroCuenta { get; set; }
        public string Alias { get; set; }       
    }    
}
