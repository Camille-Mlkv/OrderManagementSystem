using UserService.BusinessLogic.DTOs.Requests;

namespace UserService.BusinessLogic.Specifications.Services
{
    public interface IAccountService
    {
        Task SendConfirmationEmailAsync(string callBack,string userName);

        Task ConfirmEmailAsync(string userName);

        Task ForgotPasswordAsync(string callBack, string userName);

        Task<string> GetPasswordResetCodeAsync(string userName);

        Task ResetPasswordAsync(ResetPasswordRequest request);
    }
}
