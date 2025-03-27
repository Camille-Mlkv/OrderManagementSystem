using Hangfire.Dashboard;

namespace CartService.Infrastructure.Extensions
{
    public class HangfireDashboardAuthFilter: IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
