using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using WalkDifficulty = NZWalks.API.Models.Domain.WalkDifficulty;

namespace NZWalks.API.Profiles;

public class WalksProfile : Profile
{
    public WalksProfile()
    {
        CreateMap<Walk, WalkResponseDto>()
            .ReverseMap();
        CreateMap<Walk, WalkRequestDto>()
            .ReverseMap();
        CreateMap<WalkDifficulty, Models.DTO.WalkDifficulty>()
            .ReverseMap();
    }
}