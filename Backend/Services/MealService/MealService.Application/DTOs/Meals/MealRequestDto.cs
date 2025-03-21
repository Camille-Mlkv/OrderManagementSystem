﻿namespace MealService.Application.DTOs.Meals
{
    public class MealRequestDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public bool IsAvailable { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageContentType { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CuisineId { get; set; }
        public List<Guid> TagIds { get; set; } = new List<Guid>();
    }
}
