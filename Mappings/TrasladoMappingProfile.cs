using AutoMapper;
using TransportesGenesis.DTOs.Traslado;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Mappings
{
    public class TrasladoMappingProfile : Profile
    {
        public TrasladoMappingProfile()
        {
            CreateMap<SolicitudTraslado, SolicitudTrasladoDto>()
                .ForMember(dest => dest.NombreAlumno, 
                    opt => opt.MapFrom(src => src.Alumno != null 
                        ? $"{src.Alumno.Nombre} {src.Alumno.Apellido}" 
                        : "Desconocido"))
                .ForMember(dest => dest.PlacaBusOrigen, 
                    opt => opt.MapFrom(src => src.BusOrigen != null ? src.BusOrigen.Placa : "N/A"))
                .ForMember(dest => dest.PlacaBusDestino, 
                    opt => opt.MapFrom(src => src.BusDestino != null ? src.BusDestino.Placa : null))
                .ForMember(dest => dest.FechaRegistro, 
                    opt => opt.MapFrom(src => src.FechaRegistro));

            CreateMap<CrearSolicitudTrasladoDto, SolicitudTraslado>()
                .ForMember(dest => dest.IdSolicitud, opt => opt.Ignore())
                .ForMember(dest => dest.Alumno, opt => opt.Ignore())
                .ForMember(dest => dest.BusOrigen, opt => opt.Ignore())
                .ForMember(dest => dest.BusDestino, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore())
                .ForMember(dest => dest.AprobadoPor, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRespuesta, opt => opt.Ignore())
                .ForMember(dest => dest.ComentarioAdmin, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore())
                .ForMember(dest => dest.IdBusOrigen, opt => opt.Ignore()); // Se establece en el Service
        }
    }
}
