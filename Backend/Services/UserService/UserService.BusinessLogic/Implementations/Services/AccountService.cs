﻿using UserService.BusinessLogic.Exceptions;
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
        public async Task SendConfirmationEmailAsync(string callBack, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user=await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName, cancellationToken);
            if (user is null)
            {
                throw new BadRequestException("Bad request", $"User with username {userName} doesn't exist.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            await _emailService.SendEmailAsync(userName, "Confirm your account",
                        $"Confirm your account on OMS through this link: <a href='{callBack}'>Link</a>", cancellationToken);
        }

        public async Task ConfirmEmailAsync(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName, cancellationToken);
            if (user is null)
            {
                throw new BadRequestException("Bad request", $"User {userName} doesn't exist.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var confirmationToken = await _unitOfWork.UserRepository.GenerateEmailConfirmationTokenAsync(user, cancellationToken);
            if (confirmationToken is null)
            {
                throw new BadRequestException("Bad request", $"User {userName} doesn't exist.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var result = await _unitOfWork.UserRepository.ConfirmUserAccountAsync(user,confirmationToken, cancellationToken);
            if (!result.Succeeded)
            {
                var error=result.Errors.FirstOrDefault();
                throw new BadRequestException(error!.Code,error.Description);
            }

        }
        public async Task ForgotPasswordAsync(string callBack, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName, cancellationToken);
            if (user is null)
            {
                throw new BadRequestException("Bad request", $"User with username {userName} doesn't exist.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            await _emailService.SendEmailAsync(userName, "Reset your password",
                        $"To reset your password follow this link where you will find password reset code: <a href='{callBack}'>Link</a>", cancellationToken);
        }

        public async Task<string> GetPasswordResetCodeAsync(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName, cancellationToken);
            if (user is null)
            {
                throw new BadRequestException("Bad request", $"User with username {userName} doesn't exist.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var code = await _unitOfWork.UserRepository.GeneratePasswordResetCodeAsync(user, cancellationToken);
            return code;
            
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Email, cancellationToken);
            if (user is null)
            {
                throw new BadRequestException("Bad request", $"User with username {request.Email} doesn't exist.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var result = await _unitOfWork.UserRepository.ResetPasswordAsync(user, request.Code, request.Password, cancellationToken);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                throw new BadRequestException(error!.Code, error.Description);
            }

        }
    }
}
