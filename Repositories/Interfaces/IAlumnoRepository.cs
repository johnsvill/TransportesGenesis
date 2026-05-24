using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Repositories.Interfaces
{
    public interface IAlumnoRepository
    {
        Task<Alumnos?> GetByIdAsync(int id);
        Task<List<Alumnos>> GetAllAsync();
        Task<List<Alumnos>> GetAlumnosByPadreIdAsync(int idPadre);
        Task<List<Alumnos>> GetAlumnosByPadreUserIdAsync(string userId);
        Task<List<Alumnos>> GetAlumnosByBusAsync(int idBus);
        Task<Alumnos> AddAsync(Alumnos alumno);
        Task UpdateAsync(Alumnos alumno);
        Task DeleteAsync(int id);
    }
}
