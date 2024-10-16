using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("Pagos", Schema = "genesis")]
    public class Pago : Auditoria
    {
        [Key]
        public int IdPago { get; set; }

        [ForeignKey("IdAlumno")]
        public Alumnos Alumnos { get; set; }

        [ForeignKey("IdPadre")]
        public Padres Padres { get; set; }

        [ForeignKey("IdTipoCuenta")]
        public TipoCuenta TipoCuenta { get; set; }

        [ForeignKey("IdTipoRecorrido")]
        public TipoRecorridoPago TipoRecorridoPago { get; set; }

        public int MesPagado { get; set; }

        public int Anio { get; set; }   

        public bool PagoCompleto { get; set; }

        public decimal MontoParcial {  get; set; }

        public string Imagen { get; set; }

        public string Ubicacion { get; set; }

        public DateTime FechaModif { get; set; }             
    }
}
