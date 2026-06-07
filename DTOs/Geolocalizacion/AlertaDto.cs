namespace TransportesGenesis.DTOs.Geolocalizacion
{
    public class AlertaDto
    {
        public int IdAlerta { get; set; }
        public string TipoAlerta { get; set; }
        public string Mensaje { get; set; }
        public string? NombreRemitente { get; set; }
        public string? NombreDestinatario { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Leida { get; set; }
        public DateTime? FechaLectura { get; set; }
    }

    public class AlertaCreateDto
    {
        public string TipoAlerta { get; set; }
        public string Mensaje { get; set; }
        public string? IdDestinatario { get; set; }
    }

    public class AlertaMarcarLeidaDto
    {
        public int IdAlerta { get; set; }
    }
}
