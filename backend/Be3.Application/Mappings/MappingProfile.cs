using AutoMapper;
using Be3.Application.DTOs;
using Be3.Domain.Models;

namespace Be3.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Paciente, PacienteDTO>()
            .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => (int)src.Genero))
            .ForMember(dest => dest.UfRG, opt => opt.MapFrom(src => (int)src.UfRG))
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo));

        CreateMap<Convenio, ConvenioDTO>();

        CreateMap<CreatePacienteDTO, Paciente>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => (Genero)src.Genero))
            .ForMember(dest => dest.UfRG, opt => opt.MapFrom(src => (Uf)src.UfRG))
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Convenio, opt => opt.Ignore());

        CreateMap<UpdatePacienteDTO, Paciente>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => (Genero)src.Genero))
            .ForMember(dest => dest.UfRG, opt => opt.MapFrom(src => (Uf)src.UfRG))
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.Convenio, opt => opt.Ignore());
    }
}
