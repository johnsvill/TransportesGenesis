namespace TransportesGenesis.DTOs.Geolocalizacion
{
    public class BusGeolocalizacionDto
    {
        public int IdBus { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Conductor { get; set; } = string.Empty;
        public string RutaNombre { get; set; } = string.Empty;
        public string RutaDescripcion { get; set; } = string.Empty;
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int Velocidad { get; set; } // km/h
        public int Direccion { get; set; } // Grados (0-360)
        public string Estado { get; set; } = string.Empty; // "En Movimiento", "Detenido", "Sin Señal"
        public DateTime UltimaActualizacion { get; set; }
        public List<ParadaGeolocalizacionDto> Paradas { get; set; } = new();
        public int CapacidadTotal { get; set; }
        public int EstudiantesAbordo { get; set; }
    }

    public class ParadaGeolocalizacionDto
    {
        public string Nombre { get; set; } = string.Empty;
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public bool Completada { get; set; }
    }
}