using MealService.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MealService.Application.DTOs
{
    public class MealDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
        public Guid CategoryId { get; set; }
    }
}
