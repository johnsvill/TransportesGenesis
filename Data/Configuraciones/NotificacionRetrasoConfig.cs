using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class NotificacionRetrasoConfig : IEntityTypeConfiguration<NotificacionRetraso>
    {
        public void Configure(EntityTypeBuilder<NotificacionRetraso> builder)
        {
            builder.Property(x => x.TiempoRetrasoMinutos).IsRequired();
            builder.Property(x => x.Motivo).HasMaxLength(100).IsRequired();
            builder.Property(x => x.FechaHora).IsRequired();
            builder.Property(x => x.NotificadoAPadres).IsRequired();

            builder.HasIndex(x => x.FechaHora);
            builder.HasIndex(x => x.NotificadoAPadres);
        }
    }
}
