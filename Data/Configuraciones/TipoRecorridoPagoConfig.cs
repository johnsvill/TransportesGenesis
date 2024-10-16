using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class TipoRecorridoPagoConfig : IEntityTypeConfiguration<TipoRecorridoPago>
    {
        public void Configure(EntityTypeBuilder<TipoRecorridoPago> builder)
        {
            builder.Property(x => x.Precio).HasPrecision(18, 2);

            builder.HasOne(x => x.IdAlumno)
                .WithOne()
                .HasForeignKey<Alumnos>(x => x.IdAlumno);                
        }
    }
}
