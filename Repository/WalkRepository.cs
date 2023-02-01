using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository;

public class WalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext _dbContext;

    public WalkRepository(NZWalksDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<IEnumerable<Walk>> GetAllAsync()
    {
        return await _dbContext
            .Walks
            .Include(x => x.Region)
            .Include(x => x.WalkDifficulty)
            .ToListAsync();
    }

    public async Task<Walk> GetAsync(Guid id)
    {
        return await _dbContext
            .Walks
            .Include(x => x.Region)
            .Include(x => x.WalkDifficulty)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Walk> AddAsync(Walk walk)
    {
        walk.Id = Guid.NewGuid();
        await _dbContext.AddAsync(walk);
        await _dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<Walk> DeleteAsync(Walk walk)
    {
        _dbContext.Walks.Remove(walk);
        await _dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<Walk> UpdateAsync(Walk existingWalk, Walk updatedWalk)
    {
        existingWalk.Name = updatedWalk.Name;
        existingWalk.Length = updatedWalk.Length;
        existingWalk.RegionId = updatedWalk.RegionId;
        existingWalk.WalkDifficultyId = updatedWalk.WalkDifficultyId;

        await _dbContext.SaveChangesAsync();
        return existingWalk;
    }
}