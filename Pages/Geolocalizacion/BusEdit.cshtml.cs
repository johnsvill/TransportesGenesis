using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Pages.Geolocalizacion
{
    [Authorize(Roles = "Administrador")]
    public class BusEditModel : PageModel
    {
        private readonly IBusService _busService;

        public BusEditModel(IBusService busService)
        {
            _busService = busService;
        }

        [BindProperty]
        public BusUpdateDto Bus { get; set; } = new BusUpdateDto();

        public string? MensajeError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var bus = await _busService.GetBusByIdAsync(id);
            if (bus == null)
            {
                return RedirectToPage("./BusesIndex");
            }

            Bus = new BusUpdateDto
            {
                IdBus = bus.IdBus,
                Placa = bus.Placa,
                Modelo = bus.Modelo,
                Capacidad = bus.Capacidad,
                Estado = bus.Estado
            };

            return Page();
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
                await _busService.UpdateBusAsync(Bus);
                TempData["Mensaje"] = "Bus actualizado correctamente.";
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
