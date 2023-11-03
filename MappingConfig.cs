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
        
        //VillaNumber Mappings
        
        CreateMap<VillaNumber, VillaNumberDto>();
        CreateMap<VillaNumberDto, VillaNumber>();
        CreateMap<VillaNumberDto, VillaNumberCreateDto>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberUpdateDto>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberCreateDto>().ReverseMap();

    }
}