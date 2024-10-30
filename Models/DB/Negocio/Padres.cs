using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("Padres", Schema = "genesis")]
    public class Padres : Auditoria
    {
        [Key]
        public int IdPadre { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }        

        public List<Alumnos> AlumnosLink { get; set; }

        public List<TipoCuenta> TipoCuentasLink { get; set; }

        public List<Pago> PagosLink { get; set; }
    }
}
