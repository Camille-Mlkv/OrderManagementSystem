namespace MealService.Application.Specifications.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(byte[] imageData);

        Task DeleteImageAsync(string imagePublicId);

        string GetDefaultImageUrl();

        string GetPublicIdFromUrl(string imageUrl);
    }
}
