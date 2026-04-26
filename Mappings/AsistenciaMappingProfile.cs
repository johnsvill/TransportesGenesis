using AutoMapper;
using TransportesGenesis.DTOs.Asistencia;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Mappings
{
    public class AsistenciaMappingProfile : Profile
    {
        public AsistenciaMappingProfile()
        {
            // AsistenciaAlumno <-> AsistenciaDto (DTOs/Asistencia)
            CreateMap<AsistenciaAlumno, AsistenciaDto>()
                .ForMember(dest => dest.NombreAlumno, 
                    opt => opt.MapFrom(src => $"{src.Alumno.Nombre} {src.Alumno.Apellido}"));

            // AsistenciaAlumno <-> AsistenciaAlumnoDto (DTOs/Geolocalizacion)
            CreateMap<AsistenciaAlumno, AsistenciaAlumnoDto>()
                .ForMember(dest => dest.NombreAlumno, 
                    opt => opt.MapFrom(src => $"{src.Alumno.Nombre} {src.Alumno.Apellido}"));

            CreateMap<ConfirmarAsistenciaDto, AsistenciaAlumno>()
                .ForMember(dest => dest.IdAsistencia, opt => opt.Ignore())
                .ForMember(dest => dest.Alumno, opt => opt.Ignore())
                .ForMember(dest => dest.FechaConfirmacion, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore());

            CreateMap<AsistenciaCreateDto, AsistenciaAlumno>()
                .ForMember(dest => dest.IdAsistencia, opt => opt.Ignore())
                .ForMember(dest => dest.Alumno, opt => opt.Ignore())
                .ForMember(dest => dest.FechaConfirmacion, opt => opt.Ignore())
                .ForMember(dest => dest.IdBusTemporalMañana, opt => opt.Ignore())
                .ForMember(dest => dest.IdBusTemporalTarde, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore());
        }
    }
}
