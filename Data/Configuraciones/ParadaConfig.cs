using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class ParadaConfig : IEntityTypeConfiguration<Parada>
    {
        public void Configure(EntityTypeBuilder<Parada> builder)
        {
            builder.Property(x => x.Latitud).HasPrecision(10, 7).IsRequired();
            builder.Property(x => x.Longitud).HasPrecision(10, 7).IsRequired();
            builder.Property(x => x.Direccion).HasMaxLength(250);
            builder.Property(x => x.Orden).IsRequired();

            builder.HasIndex(x => x.Orden);
            builder.HasIndex(x => x.FechaRegistro);

            // Deshabilitar DELETE CASCADE
            builder.HasOne(x => x.Ruta)
                .WithMany(x => x.ParadasLink)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Alumno)
                .WithMany(x => x.ParadasLink)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
