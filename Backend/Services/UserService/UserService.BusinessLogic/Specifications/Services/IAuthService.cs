using UserService.BusinessLogic.DTOs.Requests;
using UserService.BusinessLogic.DTOs.Responses;

namespace UserService.BusinessLogic.Specifications.Services
{
    public interface IAuthService
    {
        public Task SignUpAsync(RegistrationRequest request,CancellationToken cancellationToken);

        public Task<LoginResponse> SignInAsync(LoginRequest request, CancellationToken cancellationToken);

        public Task<LoginResponse> RefreshAccessTokenAsync(RefreshAccessTokenRequest request, CancellationToken cancellationToken);

        public Task RevokeRefreshTokenAsync(string userName, CancellationToken cancellationToken);
    }
}
