using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateWalkAsync(Walk walk)
        {
           await dbContext.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;

        }

        public async Task<Walk?> DeleteWalkAsync(Guid walkId)
        {
            var walkDomain = await dbContext.Walks.FindAsync(walkId);

            if (walkDomain == null)
            {
                return null;
            }

            dbContext.Remove(walkDomain);
            await dbContext.SaveChangesAsync();
            return walkDomain;
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid walkId)
        {
            var walkDomain = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(w => w.Id == walkId);

            if (walkDomain == null)
            {
                return null;
            }
            return walkDomain;
        }

        public async Task<IEnumerable<Walk>> GetWalksAsync(string? filterOn = null, string? fiterQuery = null)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(fiterQuery) == false)
            { 
                if(filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(w => w.Name.Contains(fiterQuery));
                }
                
            }



            return await walks.ToListAsync();
            //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> UpdateWalkAsync(Guid walkId, Walk walk)
        {
            var walkDomain = await dbContext.Walks.FindAsync(walkId);

            if (walkDomain == null)
            {
                return null;
            }

            walkDomain.Name = walk.Name;
            walkDomain.Description = walk.Description;
            walkDomain.LengthInKm = walk.LengthInKm;
            walkDomain.WalkImgUrl = walk.WalkImgUrl;
            walkDomain.DifficultyId = walk.DifficultyId;
            walkDomain.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();
            return walkDomain;
        }
    }
}
