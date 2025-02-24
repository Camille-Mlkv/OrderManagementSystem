using Microsoft.AspNetCore.Mvc;

namespace MealService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MealController : ControllerBase
    {
        private readonly ILogger<MealController> _logger;

        public MealController(ILogger<MealController> logger)
        {
            _logger = logger;
        }

    }
}
