using Microsoft.AspNetCore.Identity;
using UserService.DataAccess.Models;

namespace UserService.BusinessLogic.Specifications.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserById(string userId);

        Task<ApplicationUser?> GetUserByUsername(string username);

        Task<bool> CheckPassword(ApplicationUser user, string password);

        Task<IEnumerable<string>> GetUserRoles(ApplicationUser user);

        Task UpdateUser(ApplicationUser user);

        Task CreateRole(string role);

        Task<bool> RoleExists(string role);

        Task<IdentityResult> CreateUser(ApplicationUser user, string password);

        Task AddRoleToUser(ApplicationUser user, string role);
    }
}
