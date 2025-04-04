using UserService.BusinessLogic.DTOs.Requests;
using UserService.BusinessLogic.DTOs.Responses;

namespace UserService.BusinessLogic.Specifications.Services
{
    public interface IAuthService
    {
        Task SignUpAsync(RegistrationRequest request,CancellationToken cancellationToken);

        Task<LoginResponse> SignInAsync(LoginRequest request, CancellationToken cancellationToken);

        Task<LoginResponse> RefreshAccessTokenAsync(RefreshAccessTokenRequest request, CancellationToken cancellationToken);

        Task RevokeRefreshTokenAsync(string userName, CancellationToken cancellationToken);

        Task<List<string>> GetRolesAsync(CancellationToken cancellationToken);
    }
}
