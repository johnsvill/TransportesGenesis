using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class AlertaProximidadConfig : IEntityTypeConfiguration<AlertaProximidad>
    {
        public void Configure(EntityTypeBuilder<AlertaProximidad> builder)
        {
            builder.ToTable("AlertasProximidad", "genesis");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TipoAlerta)
                .HasMaxLength(20)
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(x => x.Mensaje)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.FechaHora)
                .IsRequired();

            builder.Property(x => x.Estado)
                .HasMaxLength(15)
                .HasColumnType("varchar(15)")
                .HasDefaultValue("activo")
                .IsRequired();

            builder.Property(x => x.ParadaActual)
                .HasMaxLength(200);

            builder.Property(x => x.ParadaDestino)
                .HasMaxLength(200);

            builder.Property(x => x.ConfirmacionPadre)
                .HasDefaultValue(false);

            builder.Property(x => x.IdPadre)
                .HasMaxLength(450);

            // Relaciones con claves foráneas
            builder.HasOne(x => x.Bus)
                .WithMany()
                .HasForeignKey(x => x.IdBus)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Alumno)
                .WithMany()
                .HasForeignKey(x => x.IdAlumno)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices para mejorar consultas
            builder.HasIndex(x => x.IdBus);
            builder.HasIndex(x => x.IdAlumno);
            builder.HasIndex(x => x.Estado);
            builder.HasIndex(x => x.FechaHora);
            builder.HasIndex(x => x.IdPadre);
            builder.HasIndex(x => new { x.IdBus, x.Estado }); // Índice compuesto

            // Restricción check para TipoAlerta
            builder.HasCheckConstraint("CK_AlertaProximidad_TipoAlerta", 
                "[TipoAlerta] IN ('proximidad', 'retraso')");

            // Restricción check para Estado
            builder.HasCheckConstraint("CK_AlertaProximidad_Estado", 
                "[Estado] IN ('activo', 'resuelto')");
        }
    }
}