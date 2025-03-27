using Microsoft.AspNetCore.Identity;

namespace UserService.DataAccess.Models
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
