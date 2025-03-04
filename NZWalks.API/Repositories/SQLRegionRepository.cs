using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Region> CreateRegionAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteRegionAsync(Guid regionId)
        {
            var regionDomain = await dbContext.Regions.FindAsync(regionId);

            if (regionDomain == null)
            {
                return null;
            }
            dbContext.Remove(regionDomain);
            await dbContext.SaveChangesAsync();
            return regionDomain;
        }

        public async Task<Region?> GetRegionByIdAsync(Guid regionId)
        {
            return await dbContext.Regions.FindAsync(regionId);
        }

        public async Task<IEnumerable<Region>> GetRegionsAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> UpdateRegionAsync(Guid regionId, Region region)
        {
            var regionDomain = await dbContext.Regions.FindAsync(regionId);

            if (regionDomain == null)
            {
                return null;
            }

            regionDomain.Code = region.Code;
            regionDomain.Name = region.Name;
            regionDomain.RegionImgUrl = region.RegionImgUrl;


            //dbContext.Regions.Update(regionDomain); // update is not needed since the regionDomain is already tracked by dbContext
            await dbContext.SaveChangesAsync();
            return regionDomain;

        }
    }
}
