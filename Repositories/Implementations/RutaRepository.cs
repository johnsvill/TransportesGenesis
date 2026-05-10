using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;

namespace TransportesGenesis.Repositories.Implementations
{
    public class RutaRepository : RepositoryBase<Ruta>, IRutaRepository
    {
        public RutaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ruta>> GetByBusAsync(int idBus)
        {
            return await _dbSet
                .Where(r => r.Bus.IdBus == idBus)
                .OrderBy(r => r.HoraInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ruta>> GetActivasByBusAsync(int idBus)
        {
            return await _dbSet
                .Where(r => r.Bus.IdBus == idBus && r.EsActiva)
                .OrderBy(r => r.HoraInicio)
                .ToListAsync();
        }

        public async Task<Ruta?> GetConParadasAsync(int idRuta)
        {
            return await _dbSet
                .Include(r => r.ParadasLink.OrderBy(p => p.Orden))
                    .ThenInclude(p => p.Alumno)
                .Include(r => r.Bus)
                .FirstOrDefaultAsync(r => r.IdRuta == idRuta);
        }

        public async Task<IEnumerable<Ruta>> GetByTipoAsync(string tipoRuta)
        {
            return await _dbSet
                .Where(r => r.TipoRuta == tipoRuta && r.EsActiva)
                .Include(r => r.Bus)
                .OrderBy(r => r.HoraInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ruta>> GetRutasDelDiaAsync(DateTime fecha, string tipoRuta)
        {
            // TODO PRODUCCIÓN: Agregar filtro por fecha cuando se implemente gestión de fechas de rutas
            // Actualmente solo filtra por tipo de ruta y estado activo
            // En producción debería filtrar: r.FechaRuta.Date == fecha.Date

            // ⚠️ MODO TESTING: Ordenar por cantidad de paradas descendente para priorizar rutas completas
            return await _dbSet
                .Where(r => r.TipoRuta == tipoRuta && r.EsActiva)
                .Include(r => r.Bus)
                .Include(r => r.ParadasLink.Where(p => !p.Completada))
                    .ThenInclude(p => p.Alumno)
                .OrderByDescending(r => r.ParadasLink.Count) // Priorizar rutas con más paradas
                .ThenBy(r => r.HoraInicio)
                .ToListAsync();
        }

        public async Task<Parada?> GetParadaByIdAsync(int idParada)
        {
            return await _context.Set<Parada>()
                .Include(p => p.Alumno)
                .Include(p => p.Ruta)
                .FirstOrDefaultAsync(p => p.IdParada == idParada);
        }

        public async Task<bool> MarcarParadaCompletadaAsync(int idParada, bool completada)
        {
            var parada = await _context.Set<Parada>().FindAsync(idParada);
            if (parada == null)
                return false;

            parada.Completada = completada;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
