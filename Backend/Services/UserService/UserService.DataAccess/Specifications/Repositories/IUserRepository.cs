using Microsoft.AspNetCore.Identity;
using UserService.DataAccess.Models;

namespace UserService.DataAccess.Specifications.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId, CancellationToken cancellationToken);

        Task<ApplicationUser?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);

        Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken);

        Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user, CancellationToken cancellationToken);

        Task UpdateUserAsync(ApplicationUser user, CancellationToken cancellationToken);

        Task CreateRoleAsync(string role, CancellationToken cancellationToken);

        Task<bool> RoleExistsAsync(string role, CancellationToken cancellationToken);

        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken);

        Task AddRoleToUserAsync(ApplicationUser user, string role, CancellationToken cancellationToken);

        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user, CancellationToken cancellationToken);

        Task<IdentityResult> ConfirmUserAccountAsync(ApplicationUser user,string confirmationToken, CancellationToken cancellationToken);

        Task<string> GeneratePasswordResetCodeAsync(ApplicationUser user, CancellationToken cancellationToken);

        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string resetCode, string newPassword, CancellationToken cancellationToken);

        Task<string> GetUserEmailById(string userId, CancellationToken cancellationToken);
    }
}
