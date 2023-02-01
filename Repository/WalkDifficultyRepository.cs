using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using WalkDifficulty = NZWalks.API.Models.Domain.WalkDifficulty;

namespace NZWalks.API.Repository;

public class WalkDifficultyRepository : IWalkDifficultyRepository
{
    private readonly NZWalksDbContext _dbContext;

    public WalkDifficultyRepository(NZWalksDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
    {
        return await _dbContext
            .WalkDifficulty
            .ToListAsync();
    }

    public async Task<WalkDifficulty> GetAsync(Guid id)
    {
        return await _dbContext
            .WalkDifficulty
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
    {
        walkDifficulty.Id = Guid.NewGuid();
        await _dbContext.AddAsync(walkDifficulty);
        await _dbContext.SaveChangesAsync();
        return walkDifficulty;
    }

    public async Task<WalkDifficulty> DeleteAsync(WalkDifficulty walkDifficulty)
    {
        _dbContext.WalkDifficulty.Remove(walkDifficulty);
        await _dbContext.SaveChangesAsync();
        return walkDifficulty;
    }

    public async Task<WalkDifficulty> UpdateAsync(WalkDifficulty existingWalkDifficulty,
                                                  WalkDifficulty updatedWalkDifficulty)
    {
        existingWalkDifficulty.Code = updatedWalkDifficulty.Code;
        await _dbContext.SaveChangesAsync();
        return existingWalkDifficulty;
    }
}