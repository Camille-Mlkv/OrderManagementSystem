using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.BusinessLogic.DTOs.Requests;
using UserService.BusinessLogic.Specifications.Services;

namespace UserService.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegistrationRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start signing up procedure for user {request.Email}");

            await _authService.SignUpAsync(request,cancellationToken);

            _logger.LogInformation($"Registration successful for user {request.Email}");

            return StatusCode(201);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start logging in procedure for user {request.UserName}");

            var response=await _authService.SignInAsync(request, cancellationToken);

            _logger.LogInformation($"User {request.UserName} successfully  logged in.");

            return Ok(response);
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshAccessTokenRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start refreshing access token.");

            var response=await _authService.RefreshAccessTokenAsync(request, cancellationToken);

            _logger.LogInformation($"Access token for refresh token {request.RefreshToken} is refreshed.");

            return Ok(response);
        }

        [HttpDelete("revoke")]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshToken(string username, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start revoking refresh token.");

            await _authService.RevokeRefreshTokenAsync(username, cancellationToken);

            _logger.LogInformation($"Refresh token for user {username} is revoked.");

            return Ok();
        }
    }
}
