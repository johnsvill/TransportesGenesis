using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;

namespace TransportesGenesis.Repositories.Implementations
{
    public class AsistenciaAlumnoRepository : RepositoryBase<AsistenciaAlumno>, IAsistenciaAlumnoRepository
    {
        public AsistenciaAlumnoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<AsistenciaAlumno?> GetByAlumnoYFechaAsync(int idAlumno, DateTime fecha)
        {
            return await _dbSet
                .Include(a => a.Alumno)
                    .ThenInclude(al => al.Padres)
                .FirstOrDefaultAsync(a => a.IdAlumno == idAlumno && a.Fecha.Date == fecha.Date);
        }

        public async Task<IEnumerable<AsistenciaAlumno>> GetByAlumnoYRangoAsync(int idAlumno, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _dbSet
                .Include(a => a.Alumno)
                .Where(a => a.IdAlumno == idAlumno 
                    && a.Fecha.Date >= fechaInicio.Date 
                    && a.Fecha.Date <= fechaFin.Date)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<AsistenciaAlumno>> GetConfirmadasDelDiaAsync(DateTime fecha, bool asisteMañana, bool asisteTarde)
        {
            return await _dbSet
                .Include(a => a.Alumno)
                .Where(a => a.Fecha.Date == fecha.Date 
                    && a.FechaConfirmacion != null
                    && a.AsisteMañana == asisteMañana
                    && a.AsisteTarde == asisteTarde)
                .ToListAsync();
        }

        public async Task<bool> ConfirmarAsistenciaAsync(int idAsistencia)
        {
            var asistencia = await _dbSet.FindAsync(idAsistencia);
            if (asistencia == null)
                return false;

            asistencia.FechaConfirmacion = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AsistenciaAlumno>> GetPendientesDeConfirmarAsync(DateTime fecha)
        {
            return await _dbSet
                .Include(a => a.Alumno)
                    .ThenInclude(al => al.Padres)
                .Where(a => a.Fecha.Date == fecha.Date && a.FechaConfirmacion == null)
                .ToListAsync();
        }

        public async Task<int> ContarAlumnosPendientesPorBusAsync(int idBus, DateTime fecha, bool esMañana)
        {
            var query = _dbSet
                .Include(a => a.Alumno)
                .Where(a => a.Fecha.Date == fecha.Date && a.Alumno.IdBusAsignado == idBus);

            if (esMañana)
                query = query.Where(a => a.AsisteMañana);
            else
                query = query.Where(a => a.AsisteTarde);

            return await query.CountAsync();
        }
    }
}
