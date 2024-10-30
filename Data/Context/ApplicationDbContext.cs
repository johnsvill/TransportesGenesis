using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Models.DB.Usuarios;

namespace TransportesGenesis.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext
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
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
