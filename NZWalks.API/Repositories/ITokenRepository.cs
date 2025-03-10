using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepository
    {
        string GenerateJWTToken(IdentityUser user, List<string> Roles);
    }
}
