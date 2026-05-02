using AutoMapper;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Mappings
{
    public class GeolocalizacionMappingProfile : Profile
    {
        public GeolocalizacionMappingProfile()
        {
            // Bus
            CreateMap<Bus, BusDto>()
                .ForMember(dest => dest.RutasActivas, opt => opt.MapFrom(src => src.RutasLink != null ? src.RutasLink.Count(r => r.EsActiva) : 0))
                .ForMember(dest => dest.PilotoAsignado, opt => opt.Ignore());

            CreateMap<BusCreateDto, Bus>();
            CreateMap<BusUpdateDto, Bus>();

            // Ruta
            CreateMap<Ruta, RutaDto>()
                .ForMember(dest => dest.PlacaBus, opt => opt.MapFrom(src => src.Bus != null ? src.Bus.Placa : string.Empty))
                .ForMember(dest => dest.TotalParadas, opt => opt.MapFrom(src => src.ParadasLink != null ? src.ParadasLink.Count : 0));

            CreateMap<Ruta, RutaConParadasDto>()
                .ForMember(dest => dest.PlacaBus, opt => opt.MapFrom(src => src.Bus != null ? src.Bus.Placa : string.Empty))
                .ForMember(dest => dest.TotalParadas, opt => opt.MapFrom(src => src.ParadasLink != null ? src.ParadasLink.Count : 0))
                .ForMember(dest => dest.Paradas, opt => opt.MapFrom(src => src.ParadasLink));

            CreateMap<RutaCreateDto, Ruta>()
                .ForMember(dest => dest.EsActiva, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => 1));

            CreateMap<RutaUpdateDto, Ruta>();

            // Parada
            CreateMap<Parada, ParadaDto>()
                .ForMember(dest => dest.NombreAlumno, opt => opt.MapFrom(src => src.Alumno != null ? $"{src.Alumno.Nombre} {src.Alumno.Apellido}" : string.Empty));

            CreateMap<ParadaCreateDto, Parada>()
                .ForMember(dest => dest.Completada, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => 1));

            CreateMap<ParadaUpdateDto, Parada>();

            // AsistenciaAlumno
            CreateMap<AsistenciaAlumno, AsistenciaAlumnoDto>()
                .ForMember(dest => dest.NombreAlumno, opt => opt.MapFrom(src => src.Alumno != null ? $"{src.Alumno.Nombre} {src.Alumno.Apellido}" : string.Empty));

            CreateMap<AsistenciaCreateDto, AsistenciaAlumno>()
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => 1));

            CreateMap<AsistenciaUpdateDto, AsistenciaAlumno>();

            // UbicacionBus
            CreateMap<UbicacionBusEnTiempoReal, UbicacionBusDto>()
                .ForMember(dest => dest.PlacaBus, opt => opt.MapFrom(src => src.Bus != null ? src.Bus.Placa : string.Empty));

            CreateMap<UbicacionBusCreateDto, UbicacionBusEnTiempoReal>()
                .ForMember(dest => dest.FechaHora, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UbicacionBusEnTiempoReal, UbicacionBusEnMapaDto>()
                .ForMember(dest => dest.PlacaBus, opt => opt.MapFrom(src => src.Bus != null ? src.Bus.Placa : string.Empty))
                .ForMember(dest => dest.UltimaActualizacion, opt => opt.MapFrom(src => src.FechaHora))
                .ForMember(dest => dest.Estado, opt => opt.Ignore())
                .ForMember(dest => dest.AlumnosPendientes, opt => opt.Ignore());

            // Alerta
            CreateMap<Alerta, AlertaDto>()
                .ForMember(dest => dest.NombreRemitente, opt => opt.Ignore())
                .ForMember(dest => dest.NombreDestinatario, opt => opt.Ignore());

            CreateMap<AlertaCreateDto, Alerta>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Leida, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => 1));

            // SolicitudTraslado
            CreateMap<SolicitudTraslado, SolicitudTrasladoDto>()
                .ForMember(dest => dest.NombreAlumno, opt => opt.MapFrom(src => src.Alumno != null ? $"{src.Alumno.Nombre} {src.Alumno.Apellido}" : string.Empty))
                .ForMember(dest => dest.PlacaBusOrigen, opt => opt.MapFrom(src => src.BusOrigen != null ? src.BusOrigen.Placa : string.Empty))
                .ForMember(dest => dest.PlacaBusDestino, opt => opt.Ignore());

            CreateMap<SolicitudTrasladoCreateDto, SolicitudTraslado>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => "Pendiente"))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => 1));
        }
    }
}
