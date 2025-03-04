using System.Globalization;
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
            var walkDomain = await dbContext
                .Walks.Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(w => w.Id == walkId);

            if (walkDomain == null)
            {
                return null;
            }
            return walkDomain;
        }

        public async Task<IEnumerable<Walk>> GetWalksAsync(
            string? filterOn = null,
            string? fiterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 10
        )
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if (
                string.IsNullOrWhiteSpace(filterOn) == false
                && string.IsNullOrWhiteSpace(fiterQuery) == false
            )
            {
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(w => w.Name.Contains(fiterQuery));
                }
            }

            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending
                        ? walks.OrderBy(w => w.Name)
                        : walks.OrderByDescending(w => w.Name);
                }
                else if (sortBy.Equals("length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending
                        ? walks.OrderBy(w => w.LengthInKm)
                        : walks.OrderByDescending(w => w.LengthInKm);
                }
            }

            if (pageNumber > 0)
            {
                walks = walks.Skip((pageNumber - 1) * pageSize).Take(pageSize);
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
