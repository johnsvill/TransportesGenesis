using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class UbicacionBusEnTiempoRealConfig : IEntityTypeConfiguration<UbicacionBusEnTiempoReal>
    {
        public void Configure(EntityTypeBuilder<UbicacionBusEnTiempoReal> builder)
        {
            builder.Property(x => x.Latitud).HasPrecision(10, 7).IsRequired();
            builder.Property(x => x.Longitud).HasPrecision(10, 7).IsRequired();
            builder.Property(x => x.FechaHora).IsRequired();
            builder.Property(x => x.Velocidad).HasPrecision(5, 2);
            builder.Property(x => x.Direccion).HasPrecision(5, 2);

            builder.HasIndex(x => x.FechaHora);
        }
    }
}
