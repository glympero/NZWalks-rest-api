using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> GetWalksAsync(string? filterOn = null, string? fiterQuery = null);
        Task<Walk?> GetWalkByIdAsync(Guid walkId);
        Task<Walk> CreateWalkAsync(Walk walk);
        Task<Walk?> UpdateWalkAsync(Guid walkId, Walk walk);
        Task<Walk?> DeleteWalkAsync(Guid walkId);
    }
}
