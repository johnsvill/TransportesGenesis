using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    public class PagoPadre
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoPago { get; set; }
        public string ComprobanteUrl { get; set; }
        public string Mes { get; set; }
        public int Anio { get; set; }
        public string EstadoAdmin { get; set; }
        public string EstadoStripe { get; set; }
        public string NumeroComprobante { get; set; }
        public DateTime FechaPago { get; set; }
    
        public int? IdBanco { get; set; }
        public int? IdCuentaUsuario { get; set; }
    }
}
