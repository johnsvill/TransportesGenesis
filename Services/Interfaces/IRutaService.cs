using TransportesGenesis.DTOs.Geolocalizacion;

namespace TransportesGenesis.Services.Interfaces
{
    public interface IRutaService
    {
        Task<IEnumerable<RutaDto>> GetRutasByBusAsync(int idBus);
        Task<RutaConParadasDto?> GetRutaConParadasAsync(int idRuta);
        Task<IEnumerable<RutaDto>> GetRutasPorTipoAsync(string tipoRuta);
        Task<RutaDto> CreateRutaAsync(RutaCreateDto dto);
        Task<RutaDto> UpdateRutaAsync(RutaUpdateDto dto);
        Task<bool> DeleteRutaAsync(int idRuta);
        Task<IEnumerable<RutaDto>> GetRutasDelDiaAsync(DateTime fecha, string tipoRuta);
        Task<bool> RecalcularRutaAsync(int idRuta);
    }
}
