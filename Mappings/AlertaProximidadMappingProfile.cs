using AutoMapper;
using TransportesGenesis.DTOs.Notificaciones;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Mappings
{
    /// <summary>
    /// Perfil de mapeo para AlertaProximidad y sus DTOs
    /// </summary>
    public class AlertaProximidadMappingProfile : Profile
    {
        public AlertaProximidadMappingProfile()
        {
            // AlertaProximidad -> AlertaProximidadDto
            CreateMap<AlertaProximidad, AlertaProximidadDto>()
                .ForMember(dest => dest.NombreBus, opt => opt.Ignore()) // Se llena manualmente en el servicio
                .ForMember(dest => dest.NombreAlumno, opt => opt.Ignore()) // Se llena manualmente en el servicio
                .ForMember(dest => dest.NombrePadre, opt => opt.Ignore()) // Se llena manualmente en el servicio
                .ForMember(dest => dest.NombreParadaActual, opt => opt.MapFrom(src => src.ParadaActual))
                .ForMember(dest => dest.NombreParadaDestino, opt => opt.MapFrom(src => src.ParadaDestino));

            // AlertaProximidadCreateDto -> AlertaProximidad
            CreateMap<AlertaProximidadCreateDto, AlertaProximidad>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaHora, opt => opt.Ignore()) // Se asigna en el servicio
                .ForMember(dest => dest.FechaResolucion, opt => opt.Ignore())
                .ForMember(dest => dest.ParadasRestantes, opt => opt.Ignore())
                .ForMember(dest => dest.ParadaActual, opt => opt.Ignore())
                .ForMember(dest => dest.ParadaDestino, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmacionPadre, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.FechaConfirmacionPadre, opt => opt.Ignore())
                .ForMember(dest => dest.IdPadre, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore()) // Se asigna en el servicio
                .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore()) // Se asigna en el servicio
                .ForMember(dest => dest.Bus, opt => opt.Ignore())
                .ForMember(dest => dest.Alumno, opt => opt.Ignore());

            // AlertaProximidadUpdateDto -> AlertaProximidad (para actualizaciones parciales)
            CreateMap<AlertaProximidadUpdateDto, AlertaProximidad>()
                .ForMember(dest => dest.TipoAlerta, opt => opt.Ignore())
                .ForMember(dest => dest.Mensaje, opt => opt.Ignore())
                .ForMember(dest => dest.FechaHora, opt => opt.Ignore())
                .ForMember(dest => dest.IdBus, opt => opt.Ignore())
                .ForMember(dest => dest.IdAlumno, opt => opt.Ignore())
                .ForMember(dest => dest.ParadasRestantes, opt => opt.Ignore())
                .ForMember(dest => dest.ParadaActual, opt => opt.Ignore())
                .ForMember(dest => dest.ParadaDestino, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmacionPadre, opt => opt.Ignore())
                .ForMember(dest => dest.FechaConfirmacionPadre, opt => opt.Ignore())
                .ForMember(dest => dest.IdPadre, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore())
                .ForMember(dest => dest.Bus, opt => opt.Ignore())
                .ForMember(dest => dest.Alumno, opt => opt.Ignore());
        }
    }
}