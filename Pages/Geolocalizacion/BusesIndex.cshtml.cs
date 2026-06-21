using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task OnGetAsync()
        {
            Buses = await _busService.GetAllBusesAsync();
        }
    }
}
