using AutoMapper;

namespace firstDotnetProject;
public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDto>();
        CreateMap<VillaDto, Villa>();
        CreateMap<VillaDto, VillaCreateDto>().ReverseMap();
        CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        CreateMap<Villa, VillaCreateDto>().ReverseMap();
        CreateMap<VillaDto, ApiResponse>().ReverseMap();
    }
}