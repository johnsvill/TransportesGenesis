namespace TransportesGenesis.DTOs.Paradas
{
    public class ParadaDto
    {
        public int IdParada { get; set; }
        public int IdRuta { get; set; }
        public int? IdAlumno { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? Direccion { get; set; }
        public int Orden { get; set; }
        public TimeSpan? HoraEstimada { get; set; }
        public bool Completada { get; set; }
        public int Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Información adicional sin navegaciones circulares
        public string? NombreRuta { get; set; }
        public string? NombreAlumno { get; set; }
    }
}
