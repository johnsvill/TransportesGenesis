using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class PadresConfig : IEntityTypeConfiguration<Padres>
    {
        public void Configure(EntityTypeBuilder<Padres> builder)
        {
            builder.Property(x => x.Nombre).HasMaxLength(50);
            builder.Property(x => x.Apellido).HasMaxLength(50);       
            builder.HasIndex(x => x.FechaRegistro);
        }
    }
}
