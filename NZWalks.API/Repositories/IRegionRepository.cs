using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetRegionsAsync();
        Task<Region?> GetRegionByIdAsync(Guid regionId);
        Task<Region> CreateRegionAsync(Region region);
        Task<Region?> UpdateRegionAsync(Guid regionId, Region region);
        Task<Region?> DeleteRegionAsync(Guid regionId);
    }
}
