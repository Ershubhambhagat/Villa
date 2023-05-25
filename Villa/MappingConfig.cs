using AutoMapper;
using Villa.Models;
using Villa.Models.Dto;

namespace Villa
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Models.Villa, VillaDTO>();
            CreateMap<VillaDTO, Models.Villa >();

            CreateMap<Models.Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaUpdateDTO,VillaDTO>().ReverseMap();// this also possible 


        }
    }
}
