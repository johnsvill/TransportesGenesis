using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Tests
{
    public class DbContextTest
    {
        public static void TestDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=(local);Database=TransportesGenesis;Trusted_Connection=True;TrustServerCertificate=True");

            using var context = new ApplicationDbContext(optionsBuilder.Options);

            // Verificar entidades antiguas
            Console.WriteLine("=== ENTIDADES EXISTENTES ===");
            Console.WriteLine($"Padres: {context.PadresDb.Count()} registros");
            Console.WriteLine($"Alumnos: {context.AlumnosDb.Count()} registros");
            Console.WriteLine($"Bancos: {context.BancosDb.Count()} registros");
            Console.WriteLine($"TipoCuenta: {context.TipoCuentasDb.Count()} registros");
            Console.WriteLine($"TipoRecorridoPago: {context.TipoRecorridoPagosDb.Count()} registros");
            Console.WriteLine($"Pagos: {context.PagosDb.Count()} registros");

            // Verificar nuevas entidades
            Console.WriteLine("\n=== NUEVAS ENTIDADES DE GEOLOCALIZACIÓN ===");
            Console.WriteLine($"Buses: {context.BusesDb.Count()} registros");
            Console.WriteLine($"Rutas: {context.RutasDb.Count()} registros");
            Console.WriteLine($"Paradas: {context.ParadasDb.Count()} registros");
            Console.WriteLine($"AsistenciaAlumno: {context.AsistenciasAlumnoDb.Count()} registros");
            Console.WriteLine($"UbicacionBusEnTiempoReal: {context.UbicacionesBusDb.Count()} registros");
            Console.WriteLine($"Alertas: {context.AlertasDb.Count()} registros");
            Console.WriteLine($"SolicitudTraslado: {context.SolicitudesTrasladoDb.Count()} registros");
            Console.WriteLine($"AsignacionPilotoBus: {context.AsignacionesPilotoBusDb.Count()} registros");
            Console.WriteLine($"NotificacionProximidad: {context.NotificacionesProximidadDb.Count()} registros");
            Console.WriteLine($"RegistroRecogida: {context.RegistrosRecogidaDb.Count()} registros");
            Console.WriteLine($"NotificacionRetraso: {context.NotificacionesRetrasoDb.Count()} registros");

            Console.WriteLine("\n✅ Todas las entidades son accesibles desde el DbContext");
        }
    }
}
