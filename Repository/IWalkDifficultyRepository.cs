using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository;

public interface IWalkDifficultyRepository
{
    Task<IEnumerable<WalkDifficulty>> GetAllAsync();
    Task<WalkDifficulty> GetAsync(Guid id);
    Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty);
    Task<WalkDifficulty> DeleteAsync(WalkDifficulty walkDifficulty);
    Task<WalkDifficulty> UpdateAsync(WalkDifficulty existingWalkDifficulty, WalkDifficulty updatedWalkDifficulty);
}