namespace TransportesGenesis.DTOs.Geolocalizacion
{
    public class UbicacionBusDto
    {
        public int IdUbicacion { get; set; }
        public int IdBus { get; set; }
        public string PlacaBus { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal? Velocidad { get; set; }
        public decimal? Direccion { get; set; }
    }

    public class UbicacionBusCreateDto
    {
        public int IdBus { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public decimal? Velocidad { get; set; }
        public decimal? Direccion { get; set; }
    }

    public class UbicacionBusEnMapaDto
    {
        public int IdBus { get; set; }
        public string PlacaBus { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public decimal? Velocidad { get; set; }
        public string Estado { get; set; } // "Movimiento", "Detenido", "SinSeñal"
        public int AlumnosPendientes { get; set; }
    }
}
