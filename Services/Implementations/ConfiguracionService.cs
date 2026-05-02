using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Services.Implementations
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string?> ObtenerValorAsync(string clave)
        {
            var config = await _context.Set<Models.DB.Negocio.ConfiguracionSistema>()
                .FirstOrDefaultAsync(c => c.Clave == clave && c.Activo);

            return config?.Valor;
        }

        public async Task<(decimal latitud, decimal longitud)> ObtenerCoordenadasColegioAsync()
        {
            var latitudStr = await ObtenerValorAsync("Colegio_Latitud");
            var longitudStr = await ObtenerValorAsync("Colegio_Longitud");

            // Valores por defecto (Centro de Bogotá) si no están configurados
            decimal latitud = 4.6850m;
            decimal longitud = -74.0480m;

            if (!string.IsNullOrEmpty(latitudStr) && decimal.TryParse(latitudStr, out var lat))
            {
                latitud = lat;
            }

            if (!string.IsNullOrEmpty(longitudStr) && decimal.TryParse(longitudStr, out var lon))
            {
                longitud = lon;
            }

            return (latitud, longitud);
        }

        public async Task<string> ObtenerDireccionColegioAsync()
        {
            var direccion = await ObtenerValorAsync("Colegio_Direccion");
            return direccion ?? "Colegio Genesis";
        }
    }
}
