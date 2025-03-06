using Microsoft.AspNetCore.Http;

namespace MealService.Application.Specifications.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);

        Task DeleteImageAsync(string imagePublicId);

        string GetDefaultImageUrl();

        string GetPublicIdFromUrl(string imageUrl);
    }
}
