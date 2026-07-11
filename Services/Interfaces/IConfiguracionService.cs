using TransportesGenesis.DTOs.Geolocalizacion;

namespace TransportesGenesis.Services.Interfaces
{
    public interface IConfiguracionService
    {
        Task<string?> ObtenerValorAsync(string clave);
        Task<(decimal latitud, decimal longitud)> ObtenerCoordenadasColegioAsync();
        Task<string> ObtenerDireccionColegioAsync();
        Task<ColegioConfigDto> ObtenerColegioAsync();
        Task GuardarColegioAsync(ColegioConfigDto colegio, string? modificadoPor = null);
    }
}
