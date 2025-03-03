using Microsoft.AspNetCore.Http;

namespace MealService.Application.DTOs.Cuisines
{
    public class CuisineRequestDto
    {
        public string Name { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
