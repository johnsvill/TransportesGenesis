using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.DTOs.Ruta;
using GeoRutaDto = TransportesGenesis.DTOs.Geolocalizacion.RutaDto;
using CalcRutaDto = TransportesGenesis.DTOs.Ruta.RutaDto;

namespace TransportesGenesis.Services.Interfaces
{
    public interface IRutaService
    {
        // Métodos existentes para gestión de rutas (usando DTOs de Geolocalizacion)
        Task<IEnumerable<GeoRutaDto>> GetRutasByBusAsync(int idBus);
        Task<RutaConParadasDto?> GetRutaConParadasAsync(int idRuta);
        Task<IEnumerable<GeoRutaDto>> GetRutasPorTipoAsync(string tipoRuta);
        Task<GeoRutaDto> CreateRutaAsync(RutaCreateDto dto);
        Task<GeoRutaDto> UpdateRutaAsync(RutaUpdateDto dto);
        Task<bool> DeleteRutaAsync(int idRuta);
        Task<IEnumerable<GeoRutaDto>> GetRutasDelDiaAsync(DateTime fecha, string tipoRuta);
        Task<bool> RecalcularRutaAsync(int idRuta);

        // Nuevos métodos para FASE 5: Cálculo Dinámico (usando DTOs de Ruta)
        Task<CalcRutaDto> CalcularRutaOptimizadaAsync(CalcularRutaDto dto);
        Task<bool> MarcarParadaCompletadaAsync(MarcarParadaDto dto, string? confirmadoPor = null);
        Task<CalcRutaDto?> GetRutaActivaDelBusAsync(int idBus, DateTime fecha, string tipoRuta);
    }
}
