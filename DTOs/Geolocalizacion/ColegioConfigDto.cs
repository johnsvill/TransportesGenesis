namespace TransportesGenesis.DTOs.Geolocalizacion
{
    /// <summary>
    /// Colegio principal (parada fija). Escalable a N colegios más adelante.
    /// </summary>
    public class ColegioConfigDto
    {
        public string Nombre { get; set; } = "Colegio Genesis";
        public string Direccion { get; set; } = "";
        public decimal Latitud { get; set; } = 14.6235m;
        public decimal Longitud { get; set; } = -90.4956m;
        public string HoraInicioClases { get; set; } = "07:00";
        public string HoraFinClases { get; set; } = "14:30";
    }
}
