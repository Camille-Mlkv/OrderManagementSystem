using Microsoft.AspNetCore.Mvc;
using UserService.BusinessLogic.Specifications.Services;
using UserService.BusinessLogic.DTOs.Requests;

namespace UserService.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("{userName}/confirmation-email")]
        public async Task<IActionResult> SendConfirmationEmail(string userName, CancellationToken cancellationToken)
        {
            var callBack = $"https://localhost:5000/account/{userName}/email/confirmed";

            await _accountService.SendConfirmationEmailAsync(callBack!,userName, cancellationToken);

            _logger.LogInformation($"Confirmation email sent to {userName}");

            return Ok();
        }

        [HttpGet("{userName}/email/confirmed")]
        public async Task<IActionResult> ConfirmEmail([FromRoute]string userName, CancellationToken cancellationToken)
        {
            await _accountService.ConfirmEmailAsync(userName, cancellationToken);

            _logger.LogInformation($"User {userName} confirmed their account.");

            return Redirect("http://localhost:4200/email-confirmed");
        }

        [HttpPost("{userName}/password-email")]
        public async Task<IActionResult> ForgotPassword(string userName, CancellationToken cancellationToken)
        {
            var callBack = $"http://localhost:4200/reset-password?userName={userName}";

            await _accountService.ForgotPasswordAsync(callBack!,userName, cancellationToken);

            _logger.LogInformation($"Password reset email sent to {userName}");

            return Ok();
        }

        [HttpGet("{userName}/reset-code")]
        public async Task<IActionResult> GetPasswordResetCode([FromRoute]string userName, CancellationToken cancellationToken)
        {
            var resetCode = await _accountService.GetPasswordResetCodeAsync(userName, cancellationToken);

            _logger.LogInformation($"User {userName} received password reset email.");

            return Ok(resetCode);
        }

        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model, CancellationToken cancellationToken)
        {
            await _accountService.ResetPasswordAsync(model, cancellationToken);

            _logger.LogInformation($"Password is successfully reset for user {model.Email}");

            return Ok();
        }

        [HttpGet("users/{role}")]
        public async Task<IActionResult> GetUsersByRole(string role, CancellationToken cancellationToken)
        {
            var users = await _accountService.GetUsersByRoleAsync(role, cancellationToken);

            return Ok(users);
        }
    }
}
