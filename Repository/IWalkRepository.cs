using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository;

public interface IWalkRepository
{
    Task<IEnumerable<Walk>> GetAllAsync();
    Task<Walk> GetAsync(Guid id);
    Task<Walk> AddAsync(Walk walk);
    Task<Walk> DeleteAsync(Walk walk);
    Task<Walk> UpdateAsync(Walk existingWalk, Walk updatedWalk);
}