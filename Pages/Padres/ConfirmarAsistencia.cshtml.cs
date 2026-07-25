using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TransportesGenesis.Repositories.Interfaces;

namespace TransportesGenesis.Pages.Padres
{
    [Authorize(Roles = "PadreDeFamilia")]
    public class ConfirmarAsistenciaModel : PageModel
    {
        private readonly IAlumnoRepository _alumnoRepo;

        public int IdAlumno { get; set; } = 0;
        public string NombreAlumno { get; set; } = string.Empty;

        public ConfirmarAsistenciaModel(IAlumnoRepository alumnoRepo)
        {
            _alumnoRepo = alumnoRepo;
        }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return;

            var hijos = await _alumnoRepo.GetAlumnosByPadreUserIdAsync(userId);
            if (hijos == null || !hijos.Any()) return;

            var primerHijo = hijos.First();
            IdAlumno = primerHijo.IdAlumno;
            NombreAlumno = $"{primerHijo.Nombre} {primerHijo.Apellido}".Trim();
        }
    }
}
