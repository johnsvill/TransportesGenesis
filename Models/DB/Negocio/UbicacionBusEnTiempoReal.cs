using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("UbicacionBusEnTiempoReal", Schema = "genesis")]
    public class UbicacionBusEnTiempoReal
    {
        [Key]
        public int IdUbicacion { get; set; }

        [Required]
        public int IdBus { get; set; }

        [ForeignKey("IdBus")]
        public Bus Bus { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 7)")]
        public decimal Latitud { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 7)")]
        public decimal Longitud { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Velocidad { get; set; } // km/h

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Direccion { get; set; } // Grados (0-360)
    }
}
