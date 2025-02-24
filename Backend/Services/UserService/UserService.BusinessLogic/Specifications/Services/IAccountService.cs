using UserService.BusinessLogic.DTOs.Requests;

namespace UserService.BusinessLogic.Specifications.Services
{
    public interface IAccountService
    {
        Task SendConfirmationEmailAsync(string callBack,string userName, CancellationToken cancellationToken);

        Task ConfirmEmailAsync(string userName, CancellationToken cancellationToken);

        Task ForgotPasswordAsync(string callBack, string userName, CancellationToken cancellationToken);

        Task<string> GetPasswordResetCodeAsync(string userName, CancellationToken cancellationToken);

        Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken);
    }
}
