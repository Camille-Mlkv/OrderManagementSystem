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

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendConfirmationEmail(string userName)
        {
            var callBack = Url.Action("ConfirmEmail", "Account", new { userName }, Request.Scheme);

            await _accountService.SendConfirmationEmailAsync(callBack!,userName);
            _logger.LogInformation($"Confirmation email sent to {userName}");
            return Ok();
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string userName)
        {
            await _accountService.ConfirmEmailAsync(userName);
            _logger.LogInformation($"User {userName} confirmed their account.");
            return Ok("Account is confirmed.");
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string userName)
        {
            var callBack = Url.Action("GetPasswordResetCode", "Account", new { userName }, Request.Scheme);

            await _accountService.ForgotPasswordAsync(callBack!,userName);
            _logger.LogInformation($"Password reset email sent to {userName}");
            return Ok();
        }

        [HttpPost("GetPasswordResetCode")]
        public async Task<IActionResult> GetPasswordResetCode([FromQuery]string userName)
        {
            var resetCode=await _accountService.GetPasswordResetCodeAsync(userName);
            _logger.LogInformation($"User {userName} received password reset email.");
            return Ok(resetCode);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            await _accountService.ResetPasswordAsync(model);
            _logger.LogInformation($"Password is successfully reset for user {model.Email}");
            return Ok();
        }

    }
}
