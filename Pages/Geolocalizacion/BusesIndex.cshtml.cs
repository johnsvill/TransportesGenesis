using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Pages.Geolocalizacion
{
    [Authorize(Roles = "Administrador")]
    public class BusesIndexModel : PageModel
    {
        private readonly IBusService _busService;

        public BusesIndexModel(IBusService busService)
        {
            _busService = busService;
        }

        public IEnumerable<BusDto> Buses { get; set; } = new List<BusDto>();

        [BindProperty]
        public CrearBusInput Input { get; set; } = new();

        public string? Mensaje { get; set; }
        public string? TipoMensaje { get; set; }

        public async Task OnGetAsync()
        {
            Buses = await _busService.GetAllBusesAsync();
        }

        public async Task<IActionResult> OnPostCrearBusAsync()
        {
            if (string.IsNullOrWhiteSpace(Input.Placa))
            {
                Mensaje = "La placa es obligatoria.";
                TipoMensaje = "danger";
            }
            else if (Input.Capacidad <= 0)
            {
                Mensaje = "La capacidad debe ser mayor a cero.";
                TipoMensaje = "danger";
            }
            else
            {
                try
                {
                    await _busService.CreateBusAsync(new BusCreateDto
                    {
                        Placa = Input.Placa.Trim(),
                        Modelo = Input.Modelo?.Trim(),
                        Capacidad = Input.Capacidad
                    });

                    TempData["Mensaje"] = $"Bus {Input.Placa.Trim()} creado correctamente.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToPage("/Geolocalizacion/BusesIndex");
                }
                catch (Exception ex)
                {
                    Mensaje = ex.Message;
                    TipoMensaje = "danger";
                }
            }

            Buses = await _busService.GetAllBusesAsync();
            return Page();
        }
    }

    public class CrearBusInput
    {
        [Required]
        public string Placa { get; set; } = "";

        public string? Modelo { get; set; }

        [Range(1, 100)]
        public int Capacidad { get; set; } = 30;
    }
}
