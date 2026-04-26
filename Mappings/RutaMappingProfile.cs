using AutoMapper;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.DTOs.Ruta;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Mappings
{
    public class RutaMappingProfile : Profile
    {
        public RutaMappingProfile()
        {
            // Mapeos para DTOs de Geolocalizacion (gestión existente)
            CreateMap<Ruta, DTOs.Geolocalizacion.RutaDto>()
                .ForMember(dest => dest.PlacaBus, opt => opt.MapFrom(src => src.Bus != null ? src.Bus.Placa : $"BUS-{src.IdBus:D3}"))
                .ForMember(dest => dest.TotalParadas, opt => opt.MapFrom(src => src.ParadasLink != null ? src.ParadasLink.Count : 0));

            CreateMap<DTOs.Geolocalizacion.RutaCreateDto, Ruta>()
                .ForMember(dest => dest.IdRuta, opt => opt.Ignore())
                .ForMember(dest => dest.EsActiva, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<DTOs.Geolocalizacion.RutaUpdateDto, Ruta>()
                .ForMember(dest => dest.IdBus, opt => opt.Ignore())
                .ForMember(dest => dest.TipoRuta, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore());

            CreateMap<Ruta, DTOs.Geolocalizacion.RutaConParadasDto>()
                .IncludeBase<Ruta, DTOs.Geolocalizacion.RutaDto>()
                .ForMember(dest => dest.Paradas, opt => opt.MapFrom(src => src.ParadasLink));

            CreateMap<Parada, ParadaDto>()
                .ForMember(dest => dest.NombreAlumno, opt => opt.MapFrom(src => src.Alumno != null ? src.Alumno.Nombre : "Desconocido"));

            // Mapeos para DTOs de Ruta (FASE 5 - Cálculo Dinámico)
            CreateMap<Ruta, DTOs.Ruta.RutaDto>()
                .ForMember(dest => dest.PlacaBus, opt => opt.MapFrom(src => src.Bus != null ? src.Bus.Placa : $"BUS-{src.IdBus:D3}"))
                .ForMember(dest => dest.Paradas, opt => opt.MapFrom(src => src.ParadasLink.OrderBy(p => p.Orden)))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaRegistro));

            CreateMap<Parada, ParadaRutaDto>()
                .ForMember(dest => dest.NombreAlumno, opt => opt.MapFrom(src => src.Alumno != null ? src.Alumno.Nombre : "Desconocido"));

            CreateMap<CalcularRutaDto, Ruta>()
                .ForMember(dest => dest.IdRuta, opt => opt.Ignore())
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => $"Ruta {src.TipoRuta} - {src.Fecha:dd/MM/yyyy}"))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => $"Ruta calculada automáticamente"))
                .ForMember(dest => dest.HoraInicio, opt => opt.MapFrom(src => src.TipoRuta == "Mañana" ? new TimeSpan(6, 0, 0) : new TimeSpan(14, 0, 0)))
                .ForMember(dest => dest.EsActiva, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
