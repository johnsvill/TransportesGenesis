using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class PagoConfig : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> builder)
        {
            builder.Property(x => x.MontoParcial).HasPrecision(18, 2);
            builder.Property(x => x.Imagen).HasMaxLength(250);
            builder.Property(x => x.Ubicacion).HasMaxLength(500);
            builder.HasIndex(x => x.FechaRegistro);

            // Deshabilitar DELETE CASCADE para evitar múltiples rutas de cascada
            builder.HasOne(x => x.Alumnos)
                .WithMany(x => x.PagosLink)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Padres)
                .WithMany(x => x.PagosLink)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TipoCuenta)
                .WithMany(x => x.PagosLink)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TipoRecorridoPago)
                .WithMany(x => x.PagosLink)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
