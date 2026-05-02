using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class AlertaConfig : IEntityTypeConfiguration<Alerta>
    {
        public void Configure(EntityTypeBuilder<Alerta> builder)
        {
            builder.Property(x => x.TipoAlerta).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Mensaje).HasMaxLength(500).IsRequired();
            builder.Property(x => x.IdRemitente).HasMaxLength(450);
            builder.Property(x => x.IdDestinatario).HasMaxLength(450);
            builder.Property(x => x.FechaEnvio).IsRequired();

            builder.HasIndex(x => x.FechaEnvio);
            builder.HasIndex(x => x.IdDestinatario);
            builder.HasIndex(x => x.Leida);
        }
    }
}
