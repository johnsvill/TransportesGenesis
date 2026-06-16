using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;

namespace TransportesGenesis.Repositories.Implementations
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly ApplicationDbContext _context;

        public AlumnoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Alumnos?> GetByIdAsync(int id)
        {
            return await _context.AlumnosDb
                .Include(a => a.Padres)
                .Include(a => a.BusAsignado)
                .FirstOrDefaultAsync(a => a.IdAlumno == id);
        }

        public async Task<List<Alumnos>> GetAllAsync()
        {
            return await _context.AlumnosDb
                .Include(a => a.Padres)
                .Include(a => a.BusAsignado)
                .ToListAsync();
        }

        public async Task<List<Alumnos>> GetAllWithIncludesAsync()
        {
            return await _context.AlumnosDb
                .Include(a => a.Padres)
                .Include(a => a.BusAsignado)
                .ToListAsync();
        }

        public async Task<List<Alumnos>> GetAlumnosByPadreIdAsync(int idPadre)
        {
            return await _context.AlumnosDb
                .Include(a => a.Padres)
                .Include(a => a.BusAsignado)
                .Where(a => a.Padres.IdPadre == idPadre)
                .ToListAsync();
        }

        public async Task<List<Alumnos>> GetAlumnosByPadreUserIdAsync(string userId)
        {
            // Primero buscar el padre por UsuarioId
            var padre = await _context.PadresDb
                .FirstOrDefaultAsync(p => p.UsuarioId == userId);

            if (padre == null)
            {
                return new List<Alumnos>();
            }

            // Luego obtener sus alumnos
            return await _context.AlumnosDb
                .Include(a => a.Padres)
                .Include(a => a.BusAsignado)
                .Where(a => a.Padres.IdPadre == padre.IdPadre)
                .ToListAsync();
        }

        public async Task<List<Alumnos>> GetAlumnosByBusAsync(int idBus)
        {
            return await _context.AlumnosDb
                .Include(a => a.Padres)
                .Include(a => a.BusAsignado)
                .Where(a => a.IdBusAsignado == idBus)
                .ToListAsync();
        }

        public async Task<Alumnos> AddAsync(Alumnos alumno)
        {
            _context.AlumnosDb.Add(alumno);
            await _context.SaveChangesAsync();
            return alumno;
        }

        public async Task UpdateAsync(Alumnos alumno)
        {
            _context.AlumnosDb.Update(alumno);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var alumno = await GetByIdAsync(id);
            if (alumno != null)
            {
                _context.AlumnosDb.Remove(alumno);
                await _context.SaveChangesAsync();
            }
        }
    }
}
