using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
        Task<Region> GetAsync(Guid id);
        Task<Region> AddAsync(Region region);
        Task<Region> DeleteAsync(Region region);
        Task<Region> UpdateAsync(Region existingRegion, Region updatedRegion);
    }
}
