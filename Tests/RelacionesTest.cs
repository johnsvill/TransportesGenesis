using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;

namespace TransportesGenesis.Tests
{
    public class RelacionesTest
    {
        public static void TestRelaciones()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=(local);Database=TransportesGenesis;Trusted_Connection=True;TrustServerCertificate=True");

            using var context = new ApplicationDbContext(optionsBuilder.Options);

            Console.WriteLine("=== PRUEBA DE RELACIONES EXISTENTES ===");

            // Test 1: Padre -> Alumnos (relación antigua)
            var padreConAlumnos = context.PadresDb
                .Include(p => p.AlumnosLink)
                .FirstOrDefault();

            if (padreConAlumnos != null)
            {
                Console.WriteLine($"✅ Padre {padreConAlumnos.Nombre} tiene {padreConAlumnos.AlumnosLink?.Count ?? 0} alumnos");
            }
            else
            {
                Console.WriteLine("⚠️ No hay padres en la BD (esto es normal si es BD nueva)");
            }

            // Test 2: Alumnos -> Pagos (relación antigua)
            var alumnoConPagos = context.AlumnosDb
                .Include(a => a.PagosLink)
                .FirstOrDefault();

            if (alumnoConPagos != null)
            {
                Console.WriteLine($"✅ Alumno {alumnoConPagos.Nombre} tiene {alumnoConPagos.PagosLink?.Count ?? 0} pagos");
            }
            else
            {
                Console.WriteLine("⚠️ No hay alumnos en la BD (esto es normal si es BD nueva)");
            }

            Console.WriteLine("\n=== PRUEBA DE NUEVAS PROPIEDADES EN ALUMNOS ===");

            var alumno = context.AlumnosDb.FirstOrDefault();
            if (alumno != null)
            {
                Console.WriteLine($"✅ Alumno tiene las nuevas propiedades:");
                Console.WriteLine($"   - IdBusAsignado: {alumno.IdBusAsignado?.ToString() ?? "NULL"}");
                Console.WriteLine($"   - Latitud: {alumno.Latitud?.ToString() ?? "NULL"}");
                Console.WriteLine($"   - Longitud: {alumno.Longitud?.ToString() ?? "NULL"}");
                Console.WriteLine($"   - Direccion: {alumno.Direccion ?? "NULL"}");
            }
            else
            {
                Console.WriteLine("⚠️ No hay alumnos en la BD para verificar");
            }

            Console.WriteLine("\n=== PRUEBA DE NUEVAS RELACIONES ===");

            // Test 3: Bus -> Rutas (nueva relación)
            var busConRutas = context.BusesDb
                .Include(b => b.RutasLink)
                .FirstOrDefault();

            if (busConRutas != null)
            {
                Console.WriteLine($"✅ Bus {busConRutas.Placa} tiene {busConRutas.RutasLink?.Count ?? 0} rutas");
            }
            else
            {
                Console.WriteLine("⚠️ No hay buses (normal, acabamos de crear las tablas)");
            }

            Console.WriteLine("\n✅ TODAS LAS PRUEBAS DE RELACIONES COMPLETADAS");
        }
    }
}
