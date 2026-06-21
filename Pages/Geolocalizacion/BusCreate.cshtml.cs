using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Pages.Geolocalizacion
{
    [Authorize(Roles = "Administrador")]
    public class BusCreateModel : PageModel
    {
        private readonly IBusService _busService;

        public BusCreateModel(IBusService busService)
        {
            _busService = busService;
        }

        [BindProperty]
        public BusCreateDto Bus { get; set; } = new BusCreateDto();

        public string? MensajeError { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Bus.Placa))
            {
                MensajeError = "La placa es obligatoria.";
                return Page();
            }

            if (Bus.Capacidad <= 0)
            {
                MensajeError = "La capacidad debe ser mayor a cero.";
                return Page();
            }

            try
            {
                await _busService.CreateBusAsync(Bus);
                TempData["Mensaje"] = "Bus creado correctamente.";
                TempData["TipoMensaje"] = "success";
                return RedirectToPage("./BusesIndex");
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
                return Page();
            }
        }
    }
}
