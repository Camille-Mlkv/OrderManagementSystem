using Microsoft.AspNetCore.Identity;
using UserService.DataAccess.Models;
using UserService.BusinessLogic.Specifications.Repositories;


namespace UserService.BusinessLogic.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task AddRoleToUser(ApplicationUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> CheckPassword(ApplicationUser user, string password)
        {
            bool isCorrect = await _userManager.CheckPasswordAsync(user, password);
            return isCorrect;
        }

        public async Task CreateRole(string role)
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }

        public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
        {
            var result=await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<ApplicationUser?> GetUserById(string userId)
        {
            var applicationUser = await _userManager.FindByIdAsync(userId);
            return applicationUser;
        }

        public async Task<ApplicationUser?> GetUserByUsername(string username)
        {
            var applicationUser = await _userManager.FindByEmailAsync(username);
            return applicationUser;
        }

        public async Task<IEnumerable<string>> GetUserRoles(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<bool> RoleExists(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task UpdateUser(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }
    }
}
