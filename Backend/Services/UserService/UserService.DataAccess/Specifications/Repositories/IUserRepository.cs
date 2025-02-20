using Microsoft.AspNetCore.Identity;
using UserService.DataAccess.Models;

namespace UserService.DataAccess.Specifications.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId);

        Task<ApplicationUser?> GetUserByUsernameAsync(string username);

        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user);

        Task UpdateUserAsync(ApplicationUser user);

        Task CreateRoleAsync(string role);

        Task<bool> RoleExistsAsync(string role);

        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

        Task AddRoleToUserAsync(ApplicationUser user, string role);

        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);

        Task<IdentityResult> ConfirmUserAccountAsync(ApplicationUser user,string confirmationToken);

        Task<string> GeneratePasswordResetCodeAsync(ApplicationUser user);

        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string resetCode, string newPassword);
    }
}
