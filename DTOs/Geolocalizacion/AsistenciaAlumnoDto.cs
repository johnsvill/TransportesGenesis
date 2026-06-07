namespace TransportesGenesis.DTOs.Geolocalizacion
{
    public class AsistenciaAlumnoDto
    {
        public int IdAsistencia { get; set; }
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; }
        public DateTime Fecha { get; set; }
        public bool AsisteMañana { get; set; }
        public bool AsisteTarde { get; set; }
        public DateTime? FechaConfirmacion { get; set; }
        public int? IdBusTemporalMañana { get; set; }
        public int? IdBusTemporalTarde { get; set; }
    }

    public class AsistenciaCreateDto
    {
        public int IdAlumno { get; set; }
        public DateTime Fecha { get; set; }
        public bool AsisteMañana { get; set; }
        public bool AsisteTarde { get; set; }
    }

    public class AsistenciaUpdateDto
    {
        public int IdAsistencia { get; set; }
        public bool AsisteMañana { get; set; }
        public bool AsisteTarde { get; set; }
        public int? IdBusTemporalMañana { get; set; }
        public int? IdBusTemporalTarde { get; set; }
    }

    public class AsistenciaCalendarioDto
    {
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; }
        public Dictionary<DateTime, AsistenciaDiaDto> Calendario { get; set; } = new();
    }

    public class AsistenciaDiaDto
    {
        public bool AsisteMañana { get; set; }
        public bool AsisteTarde { get; set; }
        public bool Confirmado { get; set; }
    }
}
