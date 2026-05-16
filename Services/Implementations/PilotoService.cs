using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Services.Implementations
{
    public class PilotoService : IPilotoService
    {
        private readonly ApplicationDbContext _context;

        public PilotoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AsignacionPilotoBus?> GetAsignacionActualAsync(string idUsuarioPiloto)
        {
            return await _context.AsignacionesPilotoBusDb
                .Where(a => a.IdUsuarioPiloto == idUsuarioPiloto && a.EsActual)
                .Include(a => a.Bus)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetIdBusAsignadoAsync(string idUsuarioPiloto)
        {
            var asignacion = await GetAsignacionActualAsync(idUsuarioPiloto);
            return asignacion?.IdBus;
        }
    }
}
