namespace MealService.Application.DTOs.Cuisines
{
    public class CuisineRequestDto
    {
        public string Name { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageContentType { get; set; }
    }
}
