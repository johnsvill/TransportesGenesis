using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class BusConfig : IEntityTypeConfiguration<Bus>
    {
        public void Configure(EntityTypeBuilder<Bus> builder)
        {
            builder.Property(x => x.Placa).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Modelo).HasMaxLength(50);
            builder.Property(x => x.Capacidad).IsRequired();
            builder.Property(x => x.Estado).IsRequired();

            builder.HasIndex(x => x.Placa).IsUnique();
            builder.HasIndex(x => x.FechaRegistro);
        }
    }
}
