namespace TransportesGenesis.DTOs.Geolocalizacion
{
    public class RutaDto
    {
        public int IdRuta { get; set; }
        public int IdBus { get; set; }
        public string PlacaBus { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string TipoRuta { get; set; } // "Ida" o "Vuelta"
        public TimeSpan HoraInicio { get; set; }
        public bool EsActiva { get; set; }
        public int TotalParadas { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class RutaCreateDto
    {
        public int IdBus { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string TipoRuta { get; set; }
        public TimeSpan HoraInicio { get; set; }
    }

    public class RutaUpdateDto
    {
        public int IdRuta { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public bool EsActiva { get; set; }
    }

    public class RutaConParadasDto : RutaDto
    {
        public List<ParadaDto> Paradas { get; set; } = new();
    }
}
