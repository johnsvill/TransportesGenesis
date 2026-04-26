using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("Buses", Schema = "genesis")]
    public class Bus : Auditoria
    {
        [Key]
        public int IdBus { get; set; }

        [Required]
        [StringLength(20)]
        public string Placa { get; set; }

        [StringLength(50)]
        public string? Modelo { get; set; }

        [Range(1, 100)]
        public int Capacidad { get; set; }

        public bool Estado { get; set; } = true; // Activo/Inactivo

        // Navegación
        public List<Ruta> RutasLink { get; set; }
        public List<UbicacionBusEnTiempoReal> UbicacionesLink { get; set; }
        public AsignacionPilotoBus? AsignacionPilotoLink { get; set; }
    }
}
