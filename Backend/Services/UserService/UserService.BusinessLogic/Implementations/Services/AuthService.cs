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

        public async Task SignUp(RegistrationRequest request, CancellationToken cancellationToken)
        {
            var newUser=_mapper.Map<ApplicationUser>(request);
            newUser.UserName = newUser.Email;

            var result = await _unitOfWork.UserRepository.CreateUser(newUser, request.Password);

            if (result.Errors.Any())
            {
                var error = result.Errors.First();
                throw new BadRequestException(error.Code, error.Description);
            }

            await _unitOfWork.UserRepository.AddRoleToUser(newUser, request.Role);
        }

        public async Task<LoginResponse> SignIn(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsername(request.UserName);

            if(user is null || !await _unitOfWork.UserRepository.CheckPassword(user, request.Password))
            {
                throw new UnauthorizedException("Cannot sign in.","Wrong credentials.");
            }

            if (!user.EmailConfirmed)
            {
                throw new UnauthorizedException("Cannot sign in.", " Account is not confirmed.");
            }

            var roles = await _unitOfWork.UserRepository.GetUserRoles(user);
            var (accessToken, expiry) = _tokenService.GenerateAccessToken(user, roles);

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.SaveAllAsync(cancellationToken);

            LoginResponse loginResponse = new()
            {
                User = _mapper.Map<User>(user),
                AccessToken = accessToken,
                Expiration = expiry
            };

            return loginResponse;
        }

        public async Task<LoginResponse> RefreshAccessToken(RefreshAccessTokenRequest request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal?.Identity?.Name is null)
            {
                throw new BadRequestException("Access token not refreshed.", "Old access token is invalid.");
            }

            var user = await _unitOfWork.UserRepository.GetUserByUsername(principal.Identity.Name);
            if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new BadRequestException("Access token not refreshed.", "Refresh token expired or is invalid.");
            }
            var roles = await _unitOfWork.UserRepository.GetUserRoles(user);
            var accessToken = _tokenService.GenerateAccessToken(user, roles);

            LoginResponse loginResponseDTO = new()
            {
                User = _mapper.Map<User>(user),
                AccessToken = accessToken.AccessToken,
                Expiration = accessToken.Expiry,
            };

            return loginResponseDTO;
        }

        public async Task RevokeRefreshToken(string userName, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsername(userName);
            if (user is null)
            {
                throw new NotFoundException("Error while revoking refresh token.", $"User with username {userName} not found.");
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.SaveAllAsync(cancellationToken);
        }
    }
}
