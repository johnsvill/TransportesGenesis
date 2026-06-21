using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;

namespace TransportesGenesis.Repositories.Implementations
{
    public class BusRepository : RepositoryBase<Bus>, IBusRepository
    {
        public BusRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Bus>> GetAllConRutasAsync()
        {
            return await _dbSet
                .Include(b => b.RutasLink)
                .OrderBy(b => b.Placa)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bus>> GetActivosAsync()
        {
            return await _dbSet
                .Where(b => b.Estado == true)
                .OrderBy(b => b.Placa)
                .ToListAsync();
        }

        public async Task<Bus?> GetByPlacaAsync(string placa)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.Placa == placa);
        }

        public async Task<Bus?> GetConRutasAsync(int idBus)
        {
            return await _dbSet
                .Include(b => b.RutasLink)
                .FirstOrDefaultAsync(b => b.IdBus == idBus);
        }

        public async Task<IEnumerable<Bus>> GetConRutasActivasAsync()
        {
            return await _dbSet
                .Include(b => b.RutasLink.Where(r => r.EsActiva))
                .Where(b => b.Estado == true)
                .ToListAsync();
        }
    }
}
