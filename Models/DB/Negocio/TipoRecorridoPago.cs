using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("TipoRecorridoPago", Schema = "genesis")]
    public class TipoRecorridoPago : Auditoria
    {
        [Key]
        public int IdTipoRecorrido { get; set; }

        public Alumnos IdAlumno { get; set; }

        public int TipoRecorrido { get; set; }

        public decimal Precio { get; set; } 

        public int DiaMaximoPago { get; set; }

        public List<Pago> PagosLink { get; set; }   
    }
}
