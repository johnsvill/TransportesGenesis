using System.ComponentModel.DataAnnotations;

namespace TransportesGenesis.DTOs.Ruta
{
    public class RutaDto
    {
        public int IdRuta { get; set; }
        public int IdBus { get; set; }
        public string PlacaBus { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string TipoRuta { get; set; } // "Mañana" o "Tarde"
        public TimeSpan HoraInicio { get; set; }
        public bool EsActiva { get; set; }
        public List<ParadaRutaDto> Paradas { get; set; } = new List<ParadaRutaDto>();
        public DateTime FechaCreacion { get; set; }
    }

    public class ParadaRutaDto
    {
        public int IdParada { get; set; }
        public int? IdAlumno { get; set; } // Nullable para paradas del colegio
        public string NombreAlumno { get; set; }
        public string? NombreParada { get; set; } // Nombre de la parada (para vistas agrupadas)
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? Direccion { get; set; }
        public int Orden { get; set; }
        public TimeSpan? HoraEstimada { get; set; }
        public bool Completada { get; set; }
        public List<AlumnoEnParadaDto>? Alumnos { get; set; } // Lista de alumnos en esta parada (para vistas agrupadas)
    }

    public class AlumnoEnParadaDto
    {
        public int IdAlumno { get; set; }
        public string NombreCompleto { get; set; }
        public string? Grado { get; set; }
    }

    public class CalcularRutaDto
    {
        [Required(ErrorMessage = "El ID del bus es requerido")]
        public int IdBus { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El tipo de ruta es requerido")]
        [RegularExpression("^(Mañana|Tarde)$", ErrorMessage = "El tipo debe ser Mañana o Tarde")]
        public string TipoRuta { get; set; }

        // Opcional: Coordenadas iniciales (punto de partida del bus)
        public decimal? LatitudInicio { get; set; }
        public decimal? LongitudInicio { get; set; }
    }

    public class MarcarParadaDto
    {
        [Required]
        public int IdParada { get; set; }

        [Required]
        public bool Completada { get; set; }

        public DateTime? HoraCompletada { get; set; }
    }
}
