using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class BancoConfig : IEntityTypeConfiguration<Banco>
    {
        public void Configure(EntityTypeBuilder<Banco> builder)
        {
            builder.Property(x => x.Nombre).HasMaxLength(50);
            builder.HasIndex(x => x.FechaRegistro);
        }
    }
}
