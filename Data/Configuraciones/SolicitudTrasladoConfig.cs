using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class SolicitudTrasladoConfig : IEntityTypeConfiguration<SolicitudTraslado>
    {
        public void Configure(EntityTypeBuilder<SolicitudTraslado> builder)
        {
            builder.Property(x => x.FechaTraslado).IsRequired();
            builder.Property(x => x.Motivo).HasMaxLength(250);
            builder.Property(x => x.Estado).HasMaxLength(20).IsRequired();
            builder.Property(x => x.AprobadoPor).HasMaxLength(450);
            builder.Property(x => x.ComentarioAdmin).HasMaxLength(250);

            builder.HasIndex(x => x.Estado);
            builder.HasIndex(x => x.FechaTraslado);
            builder.HasIndex(x => x.FechaRegistro);

            // Deshabilitar DELETE CASCADE
            builder.HasOne(x => x.Alumno)
                .WithMany(x => x.SolicitudesTrasladoLink)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BusOrigen)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
