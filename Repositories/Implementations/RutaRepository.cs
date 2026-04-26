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
            return await _dbSet
                .Where(r => r.TipoRuta == tipoRuta && r.EsActiva)
                .Include(r => r.Bus)
                .Include(r => r.ParadasLink.Where(p => !p.Completada))
                    .ThenInclude(p => p.Alumno)
                .OrderBy(r => r.HoraInicio)
                .ToListAsync();
        }
    }
}
