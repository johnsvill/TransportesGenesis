using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Data.Configuraciones
{
    public class AsignacionPilotoBusConfig : IEntityTypeConfiguration<AsignacionPilotoBus>
    {
        public void Configure(EntityTypeBuilder<AsignacionPilotoBus> builder)
        {
            builder.Property(x => x.IdUsuarioPiloto).HasMaxLength(450).IsRequired();
            builder.Property(x => x.FechaAsignacion).IsRequired();
            builder.Property(x => x.EsActual).IsRequired();

            builder.HasIndex(x => x.IdUsuarioPiloto);
            builder.HasIndex(x => x.FechaAsignacion);
            builder.HasIndex(x => x.EsActual);
        }
    }
}
