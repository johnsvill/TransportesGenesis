using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Models.DB.Usuarios;
using TransportesGenesis.Models.ViewModels;

namespace TransportesGenesis.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext()
        {
                
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Padres> PadresDb { get; set; }
        public virtual DbSet<Alumnos> AlumnosDb { get; set; }
        public virtual DbSet<Banco> BancosDb { get; set; }
        public virtual DbSet<TipoCuenta> TipoCuentasDb { get; set; }
        public virtual DbSet<TipoRecorridoPago> TipoRecorridoPagosDb { get; set; }
        public virtual DbSet<Pago> PagosDb { get; set; }
        public virtual DbSet<PagoPadre> PagosPadresDb { get; set; }
        public virtual DbSet<CuentaUsuario> CuentasUsuarios { get; set; } 
        public virtual DbSet<MontoPadre> MontoPadreDb { get; set; }
        public DbSet<MontoPadreViewModel> MontosPadresView { get; set; }


        // Entidades de Geolocalización
        public virtual DbSet<Bus> BusesDb { get; set; }
        public virtual DbSet<Ruta> RutasDb { get; set; }
        public virtual DbSet<Parada> ParadasDb { get; set; }
        public virtual DbSet<AsistenciaAlumno> AsistenciasAlumnoDb { get; set; }
        public virtual DbSet<UbicacionBusEnTiempoReal> UbicacionesBusDb { get; set; }
        public virtual DbSet<Alerta> AlertasDb { get; set; }
        public virtual DbSet<SolicitudTraslado> SolicitudesTrasladoDb { get; set; }
        public virtual DbSet<AsignacionPilotoBus> AsignacionesPilotoBusDb { get; set; }
        public virtual DbSet<NotificacionProximidad> NotificacionesProximidadDb { get; set; }
        public virtual DbSet<RegistroRecogida> RegistrosRecogidaDb { get; set; }
        public virtual DbSet<NotificacionRetraso> NotificacionesRetrasoDb { get; set; }
        public virtual DbSet<ConfiguracionSistema> ConfiguracionSistemaDb { get; set; }
        public virtual DbSet<AlertaProximidad> AlertasProximidadDb { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer().UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                base.OnConfiguring(optionsBuilder);
            }          
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PagoPadre>()
            .Property(p => p.Monto)
            .HasColumnType("decimal(18,2)");

            builder.Entity<MontoPadreViewModel>().HasNoKey();

            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
               .HasDiscriminator<string>("Discriminator")
               .HasValue<AppUser>("AppUser");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
