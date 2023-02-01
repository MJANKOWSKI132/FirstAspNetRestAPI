using AutoMapper;
using NZWalks.API.Models.DTO;
using WalkDifficulty = NZWalks.API.Models.Domain.WalkDifficulty;

namespace NZWalks.API.Profiles;

public class WalkDifficultyProfile : Profile
{
    public WalkDifficultyProfile()
    {
        CreateMap<WalkDifficulty, WalkDifficultyResponseDto>()
            .ReverseMap();
    }
}