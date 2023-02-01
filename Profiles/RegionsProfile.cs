using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Region = NZWalks.API.Models.Domain.Region;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile : Profile
    {
        public RegionsProfile()
        {
            CreateMap<Region, RegionResponseDto>()
                .ReverseMap();
            CreateMap<AddRegionRequestDto, Region>()
                .ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>()
                .ReverseMap();
            CreateMap<Region, Models.DTO.Region>()
                .ReverseMap();
        }
    }
}
