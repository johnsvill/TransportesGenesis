using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;

namespace TransportesGenesis.Repositories.Implementations
{
    /// <summary>
    /// Implementación del repositorio para AlertaProximidad
    /// </summary>
    public class AlertaProximidadRepository : RepositoryBase<AlertaProximidad>, IAlertaProximidadRepository
    {
        public AlertaProximidadRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AlertaProximidad>> GetAlertasPorBusAsync(int idBus, bool incluirResueltas = false)
        {
            var query = _dbSet
                .Include(a => a.Bus)
                .Include(a => a.Alumno)
                .Where(a => a.IdBus == idBus);

            if (!incluirResueltas)
            {
                query = query.Where(a => a.Estado == "activo");
            }

            return await query
                .OrderByDescending(a => a.FechaHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlertaProximidad>> GetAlertasPorAlumnoAsync(int idAlumno, bool incluirResueltas = false)
        {
            var query = _dbSet
                .Include(a => a.Bus)
                .Include(a => a.Alumno)
                .Where(a => a.IdAlumno == idAlumno);

            if (!incluirResueltas)
            {
                query = query.Where(a => a.Estado == "activo");
            }

            return await query
                .OrderByDescending(a => a.FechaHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlertaProximidad>> GetAlertasActivasPorPadreAsync(string idPadre)
        {
            return await _dbSet
                .Include(a => a.Bus)
                .Include(a => a.Alumno)
                .Where(a => a.IdPadre == idPadre && a.Estado == "activo")
                .OrderByDescending(a => a.FechaHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlertaProximidad>> GetAlertasPorTipoYEstadoAsync(string tipoAlerta, string estado)
        {
            return await _dbSet
                .Include(a => a.Bus)
                .Include(a => a.Alumno)
                .Where(a => a.TipoAlerta == tipoAlerta && a.Estado == estado)
                .OrderByDescending(a => a.FechaHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlertaProximidad>> GetAlertasAntiguasResueltasAsync(DateTime fechaLimite)
        {
            return await _dbSet
                .Where(a => a.Estado == "resuelto" && a.FechaResolucion.HasValue && a.FechaResolucion.Value < fechaLimite)
                .ToListAsync();
        }

        public async Task<bool> ConfirmarRecepcionPadreAsync(int idAlerta)
        {
            var alerta = await _dbSet.FindAsync(idAlerta);
            if (alerta == null) return false;

            alerta.ConfirmacionPadre = true;
            alerta.FechaConfirmacionPadre = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarcarComoResueltaAsync(int idAlerta)
        {
            var alerta = await _dbSet.FindAsync(idAlerta);
            if (alerta == null) return false;

            alerta.Estado = "resuelto";
            alerta.FechaResolucion = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> DeleteMultipleAsync(IEnumerable<int> ids)
        {
            var alertas = await _dbSet
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();

            if (!alertas.Any()) return 0;

            _dbSet.RemoveRange(alertas);
            await _context.SaveChangesAsync();
            return alertas.Count;
        }

        public override async Task<IEnumerable<AlertaProximidad>> GetAllAsync()
        {
            return await _dbSet
                .Include(a => a.Bus)
                .Include(a => a.Alumno)
                .OrderByDescending(a => a.FechaHora)
                .ToListAsync();
        }

        public override async Task<AlertaProximidad?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Bus)
                .Include(a => a.Alumno)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}