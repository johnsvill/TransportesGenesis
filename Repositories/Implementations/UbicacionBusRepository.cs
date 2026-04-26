using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;

namespace TransportesGenesis.Repositories.Implementations
{
    public class UbicacionBusRepository : RepositoryBase<UbicacionBusEnTiempoReal>, IUbicacionBusRepository
    {
        public UbicacionBusRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<UbicacionBusEnTiempoReal?> GetUltimaUbicacionAsync(int idBus)
        {
            return await _dbSet
                .Where(u => u.IdBus == idBus)
                .OrderByDescending(u => u.FechaHora)
                .Include(u => u.Bus)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UbicacionBusEnTiempoReal>> GetHistorialAsync(int idBus, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _dbSet
                .Where(u => u.IdBus == idBus && 
                           u.FechaHora >= fechaInicio && 
                           u.FechaHora <= fechaFin)
                .OrderBy(u => u.FechaHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<UbicacionBusEnTiempoReal>> GetUltimasUbicacionesBusesActivosAsync()
        {
            // Obtener IDs de buses activos
            var busesActivos = await _context.Set<Bus>()
                .Where(b => b.Estado == true)
                .Select(b => b.IdBus)
                .ToListAsync();

            // Para cada bus activo, obtener su última ubicación
            var ultimasUbicaciones = new List<UbicacionBusEnTiempoReal>();

            foreach (var idBus in busesActivos)
            {
                var ultimaUbicacion = await _dbSet
                    .Include(u => u.Bus)
                    .Where(u => u.IdBus == idBus)
                    .OrderByDescending(u => u.FechaHora)
                    .FirstOrDefaultAsync();

                if (ultimaUbicacion != null)
                {
                    ultimasUbicaciones.Add(ultimaUbicacion);
                }
            }

            return ultimasUbicaciones;
        }

        public async Task<bool> LimpiarHistorialAntiguoAsync(DateTime fechaLimite)
        {
            var registrosAntiguos = await _dbSet
                .Where(u => u.FechaHora < fechaLimite)
                .ToListAsync();

            if (registrosAntiguos.Any())
            {
                _dbSet.RemoveRange(registrosAntiguos);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
