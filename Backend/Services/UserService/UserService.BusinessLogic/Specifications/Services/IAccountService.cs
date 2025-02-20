using UserService.BusinessLogic.DTOs.Requests;

namespace UserService.BusinessLogic.Specifications.Services
{
    public interface IAccountService
    {
        Task SendConfirmationEmail(string callBack,string userName);

        Task ConfirmEmail(string userName);

        Task ForgotPassword(string callBack, string userName);

        Task<string> GetPasswordResetCode(string userName);

        Task ResetPassword(ResetPasswordRequest request);
    }
}
