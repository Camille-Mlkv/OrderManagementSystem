using Microsoft.EntityFrameworkCore;

namespace CartService.Infrastructure.Data
{
    public class HangfireDbContext: DbContext
    {
        public HangfireDbContext(DbContextOptions<HangfireDbContext> options) : base(options) { }
    }
}
