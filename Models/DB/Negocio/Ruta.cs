using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("Rutas", Schema = "genesis")]
    public class Ruta : Auditoria
    {
        [Key]
        public int IdRuta { get; set; }

        [ForeignKey("IdBus")]
        public Bus Bus { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(10)]
        public string TipoRuta { get; set; } // "Ida" o "Vuelta"

        public TimeSpan HoraInicio { get; set; }

        public bool EsActiva { get; set; } = true;

        // Navegación
        public List<Parada> ParadasLink { get; set; }
    }
}
