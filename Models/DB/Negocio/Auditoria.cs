namespace TransportesGenesis.Models.DB.Negocio
{
    public class Auditoria
    {
        public int Activo { get; set; } = 1;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
