using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class RegistroRecogidaConfig : IEntityTypeConfiguration<RegistroRecogida>
    {
        public void Configure(EntityTypeBuilder<RegistroRecogida> builder)
        {
            builder.Property(x => x.FechaHoraRecogida).IsRequired();
            builder.Property(x => x.ConfirmadoPor).HasMaxLength(450);
            builder.Property(x => x.Latitud).HasPrecision(10, 7);
            builder.Property(x => x.Longitud).HasPrecision(10, 7);
            builder.Property(x => x.AlumnoPresente).IsRequired();

            builder.HasIndex(x => x.FechaHoraRecogida);
            builder.HasIndex(x => x.AlumnoPresente);

            // Deshabilitar DELETE CASCADE
            builder.HasOne(x => x.Parada)
                .WithMany(x => x.RegistrosRecogidaLink)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Alumno)
                .WithMany(x => x.RegistrosRecogidaLink)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
