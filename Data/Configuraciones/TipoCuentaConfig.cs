using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class TipoCuentaConfig : IEntityTypeConfiguration<TipoCuenta>
    {
        public void Configure(EntityTypeBuilder<TipoCuenta> builder)
        {
            builder.Property(x => x.Nombre).HasMaxLength(50);
            builder.HasIndex(x => x.FechaRegistro);
        }
    }
}
