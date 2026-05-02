using TransportesGenesis.DTOs.Geolocalizacion;

namespace TransportesGenesis.Services.Interfaces
{
    public interface IUbicacionBusService
    {
        Task<UbicacionBusDto?> GetUltimaUbicacionAsync(int idBus);
        Task<IEnumerable<UbicacionBusEnMapaDto>> GetUbicacionesBusesActivosAsync();
        Task<UbicacionBusDto> RegistrarUbicacionAsync(UbicacionBusCreateDto dto);
        Task<IEnumerable<UbicacionBusDto>> GetHistorialAsync(int idBus, DateTime fechaInicio, DateTime fechaFin);
        Task<bool> LimpiarHistorialAntiguoAsync(int diasAntiguedad);
    }
}
