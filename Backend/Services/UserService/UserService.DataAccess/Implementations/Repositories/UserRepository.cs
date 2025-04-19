using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.DataAccess.Models;
using UserService.DataAccess.Specifications.Repositories;

namespace UserService.DataAccess.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task AddRoleToUserAsync(ApplicationUser user, string role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var isCorrect = await _userManager.CheckPasswordAsync(user, password);

            return isCorrect;
        }

        public async Task CreateRoleAsync(string role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _userManager.CreateAsync(user, password);

            return result;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var applicationUser = await _userManager.FindByIdAsync(userId);

            return applicationUser;
        }

        public async Task<ApplicationUser?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var applicationUser = await _userManager.FindByEmailAsync(username);

            return applicationUser;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roles = await _userManager.GetRolesAsync(user);

            return roles;
        }

        public async Task<bool> RoleExistsAsync(string role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task UpdateUserAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _userManager.UpdateAsync(user);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return token;
        }

        public async Task<IdentityResult> ConfirmUserAccountAsync(ApplicationUser user, string confirmationToken, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);

            return result;
        }

        public async Task<string> GeneratePasswordResetCodeAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            return code;
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string resetCode, string newPassword, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var resetResult = await _userManager.ResetPasswordAsync(user, resetCode, newPassword);

            return resetResult;
        }

        public async Task<string> GetUserEmailByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId);

            var email = user!.Email;

            return email!;
        }

        public async Task<List<string>> GetRolesAsync(CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
               .Select(role => role.Name)
               .ToListAsync(cancellationToken);

            return roles!;
        }

        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string role, CancellationToken cancellationToken)
        {
            var users = (await _userManager.GetUsersInRoleAsync(role)).ToList();

            return users;
        }
    }
}
