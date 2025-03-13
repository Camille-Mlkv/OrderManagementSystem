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

        [HttpPost("users/{userName}/confirmation-email")]
        public async Task<IActionResult> SendConfirmationEmail(string userName, CancellationToken cancellationToken)
        {
            var callBack = Url.Action("ConfirmEmail", "Account", new { userName }, Request.Scheme);

            await _accountService.SendConfirmationEmailAsync(callBack!,userName, cancellationToken);
            _logger.LogInformation($"Confirmation email sent to {userName}");

            return Ok();
        }

        [HttpGet("users/{userName}/email/confirmed")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string userName, CancellationToken cancellationToken)
        {
            await _accountService.ConfirmEmailAsync(userName, cancellationToken);
            _logger.LogInformation($"User {userName} confirmed their account.");

            return Ok("Account is confirmed.");
        }

        [HttpPost("users/{userName}/password-email")]
        public async Task<IActionResult> ForgotPassword(string userName, CancellationToken cancellationToken)
        {
            var callBack = Url.Action("GetPasswordResetCode", "Account", new { userName }, Request.Scheme);

            await _accountService.ForgotPasswordAsync(callBack!,userName, cancellationToken);
            _logger.LogInformation($"Password reset email sent to {userName}");

            return Ok();
        }

        [HttpGet("users/{userName}/reset-code")]
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

    }
}
