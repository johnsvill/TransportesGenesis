using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportesGenesis.Models.DB.Negocio
{
    [Table("ConfiguracionSistema", Schema = "genesis")]
    public class ConfiguracionSistema
    {
        [Key]
        public int IdConfiguracion { get; set; }

        [Required]
        [MaxLength(100)]
        public string Clave { get; set; } = string.Empty;

        [Required]
        public string Valor { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [MaxLength(50)]
        public string? Tipo { get; set; } // 'Texto', 'Numero', 'Coordenada', 'Booleano'

        [MaxLength(50)]
        public string? Categoria { get; set; } // 'General', 'Ubicacion', 'Notificaciones'

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public DateTime? UltimaModificacion { get; set; }

        [MaxLength(100)]
        public string? ModificadoPor { get; set; }
    }
}
