using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("Alumnos", Schema = "genesis")]
    public class Alumnos : Auditoria
    {
        [Key]
        public int IdAlumno { get; set; }

        [ForeignKey("IdPadre")]
        public Padres Padres { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public List<Pago> PagosLink { get; set; }
    }
}
