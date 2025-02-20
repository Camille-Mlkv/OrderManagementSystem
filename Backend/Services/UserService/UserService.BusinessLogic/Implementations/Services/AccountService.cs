using UserService.BusinessLogic.Exceptions;
using UserService.DataAccess.Specifications;
using UserService.BusinessLogic.Specifications.Services;
using UserService.BusinessLogic.DTOs.Requests;

namespace UserService.BusinessLogic.Implementations.Services
{
    public class AccountService : IAccountService
    {
        private IUnitOfWork _unitOfWork;
        private IEmailService _emailService;

        public AccountService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        public async Task SendConfirmationEmailAsync(string callBack, string userName)
        {
            var user=await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
            if (user is null)
            {
                throw new BadRequestException("Bad request", $"User with username {userName} doesn't exist.");
            }

            await _emailService.SendEmailAsync(userName, "Confirm your account",
                        $"Confirm your account on OMS through this link: <a href='{callBack}'>Link</a>");
        }

        public async Task ConfirmEmailAsync(string userName)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
            if (user is null)
            {
                throw new BadRequestException("Bad request", $"User {userName} doesn't exist.");
            }

            var confirmationToken = await _unitOfWork.UserRepository.GenerateEmailConfirmationTokenAsync(user);
            if (confirmationToken is null)
            {
                throw new BadRequestException("Bad request", $"User {userName} doesn't exist.");
            }

            var result = await _unitOfWork.UserRepository.ConfirmUserAccountAsync(user,confirmationToken);
            if (!result.Succeeded)
            {
                var error=result.Errors.FirstOrDefault();
                throw new BadRequestException(error!.Code,error.Description);
            }

        }
        public async Task ForgotPasswordAsync(string callBack, string userName)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
            if (user is null)
            {
                throw new BadRequestException("Bad request", $"User with username {userName} doesn't exist.");
            }


            await _emailService.SendEmailAsync(userName, "Reset your password",
                        $"To reset your password follow this link where you will find password reset code: <a href='{callBack}'>Link</a>");
        }

        public async Task<string> GetPasswordResetCodeAsync(string userName)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
            if (user is null)
            {
                throw new BadRequestException("Bad request", $"User with username {userName} doesn't exist.");
            }
            var code = await _unitOfWork.UserRepository.GeneratePasswordResetCodeAsync(user);
            return code;
            
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Email);
            if (user is null)
            {
                throw new BadRequestException("Bad request", $"User with username {request.Email} doesn't exist.");
            }
            var result = await _unitOfWork.UserRepository.ResetPasswordAsync(user, request.Code, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                throw new BadRequestException(error!.Code, error.Description);
            }

        }
    }
}
