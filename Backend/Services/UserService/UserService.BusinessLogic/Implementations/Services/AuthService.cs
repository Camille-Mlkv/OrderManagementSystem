using UserService.DataAccess.Models;
using UserService.BusinessLogic.Specifications.Services;
using UserService.DataAccess.Specifications;
using UserService.BusinessLogic.DTOs.Requests;
using UserService.BusinessLogic.DTOs.Responses;
using UserService.BusinessLogic.DTOs;
using AutoMapper;
using UserService.BusinessLogic.Exceptions;

namespace UserService.BusinessLogic.Implementations.Services
{
    public class AuthService : IAuthService
    {
        private IUnitOfWork _unitOfWork;
        private ITokenService _tokenService;
        private IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork,ITokenService tokenService,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task SignUpAsync(RegistrationRequest request, CancellationToken cancellationToken)
        {
            var newUser=_mapper.Map<ApplicationUser>(request);
            newUser.UserName = newUser.Email;

            cancellationToken.ThrowIfCancellationRequested();

            var result = await _unitOfWork.UserRepository.CreateUserAsync(newUser, request.Password, cancellationToken);

            if (result.Errors.Any())
            {
                var error = result.Errors.First();
                throw new BadRequestException(error.Code, error.Description);
            }

            await _unitOfWork.UserRepository.AddRoleToUserAsync(newUser, request.Role, cancellationToken);
        }

        public async Task<LoginResponse> SignInAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Email, cancellationToken);

            if (user is null || !await _unitOfWork.UserRepository.CheckPasswordAsync(user, request.Password, cancellationToken))
            {
                throw new UnauthorizedException("Cannot sign in.","Wrong credentials.");
            }

            if (!user.EmailConfirmed)
            {
                throw new UnauthorizedException("Cannot sign in.", " Account is not confirmed.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var roles = await _unitOfWork.UserRepository.GetUserRolesAsync(user, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var (accessToken, expiry) = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.UserRepository.UpdateUserAsync(user, cancellationToken);
            await _unitOfWork.SaveAllAsync(cancellationToken);

            LoginResponse loginResponse = new()
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
            };

            return loginResponse;
        }

        public async Task<LoginResponse> RefreshAccessTokenAsync(RefreshAccessTokenRequest request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal?.Identity?.Name is null)
            {
                throw new BadRequestException("Access token not refreshed.", "Old access token is invalid.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(principal.Identity.Name, cancellationToken);
            if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new BadRequestException("Access token not refreshed.", "Refresh token expired or is invalid.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var roles = await _unitOfWork.UserRepository.GetUserRolesAsync(user, cancellationToken);

            var accessTokenData = _tokenService.GenerateAccessToken(user, roles);

            LoginResponse loginResponseDTO = new()
            {
                AccessToken = accessTokenData.AccessToken,
                RefreshToken = user.RefreshToken,
            };

            return loginResponseDTO;
        }

        public async Task RevokeRefreshTokenAsync(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException("Error while revoking refresh token.", $"User with username {userName} not found.");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            cancellationToken.ThrowIfCancellationRequested();

            await _unitOfWork.UserRepository.UpdateUserAsync(user, cancellationToken);
            await _unitOfWork.SaveAllAsync(cancellationToken);
        }

        public async Task<List<string>> GetRolesAsync(CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.UserRepository.GetRolesAsync(cancellationToken);

            return roles;
        }
    }
}
