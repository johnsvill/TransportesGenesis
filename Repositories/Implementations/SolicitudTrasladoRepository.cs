using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Repositories.Interfaces;

namespace TransportesGenesis.Repositories.Implementations
{
    public class SolicitudTrasladoRepository : RepositoryBase<SolicitudTraslado>, ISolicitudTrasladoRepository
    {
        public SolicitudTrasladoRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Override para incluir navigation properties
        public override async Task<SolicitudTraslado?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Alumno)
                .Include(s => s.BusOrigen)
                .Include(s => s.BusDestino)
                .FirstOrDefaultAsync(s => s.IdSolicitud == id);
        }

        public async Task<IEnumerable<SolicitudTraslado>> GetByAlumnoAsync(int idAlumno)
        {
            return await _dbSet
                .Include(s => s.Alumno)
                .Include(s => s.BusOrigen)
                .Include(s => s.BusDestino)
                .Where(s => s.IdAlumno == idAlumno)
                .OrderByDescending(s => s.FechaTraslado)
                .ToListAsync();
        }

        public async Task<IEnumerable<SolicitudTraslado>> GetPendientesAsync()
        {
            return await _dbSet
                .Include(s => s.Alumno)
                    .ThenInclude(a => a.Padres)
                .Include(s => s.BusOrigen)
                .Include(s => s.BusDestino)
                .Where(s => s.Estado == "Pendiente")
                .OrderBy(s => s.FechaTraslado)
                .ToListAsync();
        }

        public async Task<IEnumerable<SolicitudTraslado>> GetByEstadoAsync(string estado)
        {
            return await _dbSet
                .Include(s => s.Alumno)
                    .ThenInclude(a => a.Padres)
                .Include(s => s.BusOrigen)
                .Include(s => s.BusDestino)
                .Where(s => s.Estado == estado)
                .OrderBy(s => s.FechaTraslado)
                .ToListAsync();
        }

        public async Task<bool> AprobarAsync(int idSolicitud, string idAdmin, int idBusDestino, string? comentario)
        {
            var solicitud = await _dbSet.FindAsync(idSolicitud);
            if (solicitud == null || solicitud.Estado != "Pendiente")
                return false;

            solicitud.Estado = "Aprobado";
            solicitud.IdBusDestino = idBusDestino;
            solicitud.AprobadoPor = idAdmin;
            solicitud.FechaRespuesta = DateTime.Now;
            solicitud.ComentarioAdmin = comentario;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RechazarAsync(int idSolicitud, string idAdmin, string? comentario)
        {
            var solicitud = await _dbSet.FindAsync(idSolicitud);
            if (solicitud == null || solicitud.Estado != "Pendiente")
                return false;

            solicitud.Estado = "Rechazado";
            solicitud.AprobadoPor = idAdmin;
            solicitud.FechaRespuesta = DateTime.Now;
            solicitud.ComentarioAdmin = comentario;

            await _context.SaveChangesAsync();
            return true;
        }

        // Métodos adicionales auxiliares
        public async Task<IEnumerable<SolicitudTraslado>> GetSolicitudesPorFechaAsync(DateTime fecha)
        {
            return await _dbSet
                .Include(s => s.Alumno)
                .Include(s => s.BusOrigen)
                .Include(s => s.BusDestino)
                .Where(s => s.FechaTraslado.Date == fecha.Date)
                .OrderBy(s => s.Alumno.Nombre)
                .ToListAsync();
        }

        public async Task<SolicitudTraslado?> GetSolicitudActivaAsync(int idAlumno, DateTime fecha)
        {
            return await _dbSet
                .Include(s => s.Alumno)
                .Include(s => s.BusOrigen)
                .Include(s => s.BusDestino)
                .Where(s => s.IdAlumno == idAlumno 
                    && s.FechaTraslado.Date == fecha.Date 
                    && s.Estado == "Aprobado")
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SolicitudTraslado>> GetTrasladosActivosPorFechaAsync(DateTime fecha)
        {
            return await _dbSet
                .Include(s => s.Alumno)
                .Include(s => s.BusOrigen)
                .Include(s => s.BusDestino)
                .Where(s => s.FechaTraslado.Date == fecha.Date && s.Estado == "Aprobado")
                .ToListAsync();
        }
    }
}
