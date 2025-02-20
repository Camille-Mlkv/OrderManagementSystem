using System.ComponentModel.DataAnnotations;

namespace UserService.BusinessLogic.DTOs.Requests
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}
