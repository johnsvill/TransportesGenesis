using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class NotificacionProximidadConfig : IEntityTypeConfiguration<NotificacionProximidad>
    {
        public void Configure(EntityTypeBuilder<NotificacionProximidad> builder)
        {
            builder.Property(x => x.TipoNotificacion).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Enviada).IsRequired();

            builder.HasIndex(x => x.Enviada);
            builder.HasIndex(x => x.FechaHoraEnvio);

            // Deshabilitar DELETE CASCADE
            builder.HasOne(x => x.Parada)
                .WithMany(x => x.NotificacionesLink)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Padre)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
