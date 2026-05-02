namespace TransportesGenesis.Services.Interfaces
{
    public interface IConfiguracionService
    {
        Task<string?> ObtenerValorAsync(string clave);
        Task<(decimal latitud, decimal longitud)> ObtenerCoordenadasColegioAsync();
        Task<string> ObtenerDireccionColegioAsync();
    }
}
