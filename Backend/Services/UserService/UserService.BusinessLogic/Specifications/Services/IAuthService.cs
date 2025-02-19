using UserService.BusinessLogic.DTOs.Requests;
using UserService.BusinessLogic.DTOs.Responses;

namespace UserService.BusinessLogic.Specifications.Services
{
    public interface IAuthService
    {
        public Task SignUp(RegistrationRequest request,CancellationToken cancellationToken);

        public Task<LoginResponse> SignIn(LoginRequest request, CancellationToken cancellationToken);

        public Task<LoginResponse> RefreshAccessToken(RefreshAccessTokenRequest request, CancellationToken cancellationToken);

        public Task RevokeRefreshToken(string userName, CancellationToken cancellationToken);
    }
}
