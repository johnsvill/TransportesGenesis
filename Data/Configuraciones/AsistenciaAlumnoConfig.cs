using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class AsistenciaAlumnoConfig : IEntityTypeConfiguration<AsistenciaAlumno>
    {
        public void Configure(EntityTypeBuilder<AsistenciaAlumno> builder)
        {
            builder.Property(x => x.Fecha).IsRequired();
            builder.Property(x => x.AsisteMañana).IsRequired();
            builder.Property(x => x.AsisteTarde).IsRequired();

            builder.HasIndex(x => x.Fecha);
            builder.HasIndex(x => x.FechaRegistro);

            // Deshabilitar DELETE CASCADE
            builder.HasOne(x => x.Alumno)
                .WithMany(x => x.AsistenciasLink)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones opcionales con buses temporales
            builder.HasOne(x => x.BusTemporalMañana)
                .WithMany()
                .HasForeignKey(x => x.IdBusTemporalMañana)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.BusTemporalTarde)
                .WithMany()
                .HasForeignKey(x => x.IdBusTemporalTarde)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
