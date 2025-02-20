using System.Security.Claims;
using UserService.DataAccess.Models;

namespace UserService.BusinessLogic.Specifications.Services
{
    public interface ITokenService
    {
        (string AccessToken, DateTime Expiry) GenerateAccessToken(ApplicationUser user, IEnumerable<string> roles);

        string GenerateRefreshToken();

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken);
    }
}
