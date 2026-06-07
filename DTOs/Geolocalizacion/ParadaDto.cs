namespace TransportesGenesis.DTOs.Geolocalizacion
{
    public class ParadaDto
    {
        public int IdParada { get; set; }
        public int IdRuta { get; set; }
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? Direccion { get; set; }
        public int Orden { get; set; }
        public TimeSpan? HoraEstimada { get; set; }
        public bool Completada { get; set; }
    }

    public class ParadaCreateDto
    {
        public int IdRuta { get; set; }
        public int IdAlumno { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? Direccion { get; set; }
        public int Orden { get; set; }
        public TimeSpan? HoraEstimada { get; set; }
    }

    public class ParadaUpdateDto
    {
        public int IdParada { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? Direccion { get; set; }
        public int Orden { get; set; }
        public TimeSpan? HoraEstimada { get; set; }
        public bool Completada { get; set; }
    }
}
