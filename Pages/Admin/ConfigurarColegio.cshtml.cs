using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class ConfigurarColegioModel : PageModel
    {
        private readonly IConfiguracionService _configuracionService;

        public ConfigurarColegioModel(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        [BindProperty]
        public ColegioInput Input { get; set; } = new();

        public string? Mensaje { get; set; }
        public string? TipoMensaje { get; set; }

        public async Task OnGetAsync()
        {
            var colegio = await _configuracionService.ObtenerColegioAsync();
            Input = new ColegioInput
            {
                Nombre = colegio.Nombre,
                Direccion = colegio.Direccion,
                Latitud = colegio.Latitud.ToString(CultureInfo.InvariantCulture),
                Longitud = colegio.Longitud.ToString(CultureInfo.InvariantCulture),
                HoraInicioClases = colegio.HoraInicioClases,
                HoraFinClases = colegio.HoraFinClases
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Mensaje = "Revisa los campos del formulario.";
                TipoMensaje = "danger";
                return Page();
            }

            if (!decimal.TryParse(Input.Latitud, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat) ||
                !decimal.TryParse(Input.Longitud, NumberStyles.Any, CultureInfo.InvariantCulture, out var lon))
            {
                Mensaje = "Latitud y longitud deben ser números válidos (usa punto decimal).";
                TipoMensaje = "danger";
                return Page();
            }

            try
            {
                await _configuracionService.GuardarColegioAsync(new ColegioConfigDto
                {
                    Nombre = Input.Nombre,
                    Direccion = Input.Direccion ?? "",
                    Latitud = lat,
                    Longitud = lon,
                    HoraInicioClases = Input.HoraInicioClases ?? "07:00",
                    HoraFinClases = Input.HoraFinClases ?? "14:30"
                }, User.Identity?.Name);

                TempData["Mensaje"] = "Colegio (principal) guardado. Esta ubicación se usa como parada fija en rutas y mapas.";
                TempData["TipoMensaje"] = "success";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                TipoMensaje = "danger";
                return Page();
            }
        }
    }

    public class ColegioInput
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(200)]
        public string Nombre { get; set; } = "Colegio Genesis";

        [StringLength(500)]
        public string? Direccion { get; set; }

        [Required]
        public string Latitud { get; set; } = "14.6235";

        [Required]
        public string Longitud { get; set; } = "-90.4956";

        public string? HoraInicioClases { get; set; } = "07:00";
        public string? HoraFinClases { get; set; } = "14:30";
    }
}
