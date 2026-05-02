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

        // Campos para geolocalización (nullable para no afectar registros existentes)
        public int? IdBusAsignado { get; set; }

        [ForeignKey("IdBusAsignado")]
        public Bus? BusAsignado { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Latitud { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Longitud { get; set; }

        [StringLength(250)]
        public string? Direccion { get; set; }

        // Navegación
        public List<Pago> PagosLink { get; set; }
        public List<AsistenciaAlumno> AsistenciasLink { get; set; }
        public List<Parada> ParadasLink { get; set; }
        public List<RegistroRecogida> RegistrosRecogidaLink { get; set; }
        public List<SolicitudTraslado> SolicitudesTrasladoLink { get; set; }
    }
}
