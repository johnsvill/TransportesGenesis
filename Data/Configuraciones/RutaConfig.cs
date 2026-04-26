using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class RutaConfig : IEntityTypeConfiguration<Ruta>
    {
        public void Configure(EntityTypeBuilder<Ruta> builder)
        {
            builder.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Descripcion).HasMaxLength(250);
            builder.Property(x => x.TipoRuta).HasMaxLength(10).IsRequired();
            builder.Property(x => x.HoraInicio).IsRequired();

            builder.HasIndex(x => x.FechaRegistro);
            builder.HasIndex(x => x.TipoRuta);
        }
    }
}
