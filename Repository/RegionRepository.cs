using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            var regions = await nZWalksDbContext.Regions.ToListAsync();
            return regions;
        }

        public async Task<Region> GetAsync(Guid id)
        {
            var region = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            return region;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Region region)
        {
            nZWalksDbContext.Regions.Remove(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> UpdateAsync(Region existingRegion, Region updatedRegion)
        {
            existingRegion.Code = updatedRegion.Code;
            existingRegion.Area = updatedRegion.Area;
            existingRegion.Name = updatedRegion.Name;
            existingRegion.Lat = updatedRegion.Lat;
            existingRegion.Long = updatedRegion.Long;
            existingRegion.Population = updatedRegion.Population;

            await nZWalksDbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
