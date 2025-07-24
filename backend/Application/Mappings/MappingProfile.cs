using Application.DTOs.Empleado;
using Application.DTOs.Tienda;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Empleado mappings
        CreateMap<Empleado, EmpleadoDto>()
            .ForMember(dest => dest.TiendaNombre, opt => opt.MapFrom(src => src.Tienda.Nombre));

        CreateMap<CreateEmpleadoDto, Empleado>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true));

        CreateMap<UpdateEmpleadoDto, Empleado>()
            .ForMember(dest => dest.Tienda, opt => opt.Ignore())
            .ForMember(dest => dest.Usuarios, opt => opt.Ignore());

        // Tienda mappings
        CreateMap<Tienda, TiendaDto>()
            .ForMember(dest => dest.EmpleadosCount, opt => opt.MapFrom(src => src.Empleados.Count(e => e.Estado)));

        CreateMap<CreateTiendaDto, Tienda>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Empleados, opt => opt.Ignore());

        CreateMap<UpdateTiendaDto, Tienda>()
            .ForMember(dest => dest.Empleados, opt => opt.Ignore());
    }
}
