namespace TransportesGenesis.DTOs.Geolocalizacion
{
    public class SolicitudTrasladoDto
    {
        public int IdSolicitud { get; set; }
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; }
        public int IdBusOrigen { get; set; }
        public string PlacaBusOrigen { get; set; }
        public int? IdBusDestino { get; set; }
        public string? PlacaBusDestino { get; set; }
        public DateTime FechaTraslado { get; set; }
        public string? Motivo { get; set; }
        public string Estado { get; set; }
        public string? AprobadoPor { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string? ComentarioAdmin { get; set; }
    }

    public class SolicitudTrasladoCreateDto
    {
        public int IdAlumno { get; set; }
        public int IdBusOrigen { get; set; }
        public DateTime FechaTraslado { get; set; }
        public string? Motivo { get; set; }
    }

    public class SolicitudTrasladoAprobacionDto
    {
        public int IdSolicitud { get; set; }
        public bool Aprobar { get; set; }
        public int? IdBusDestino { get; set; }
        public string? ComentarioAdmin { get; set; }
    }
}
