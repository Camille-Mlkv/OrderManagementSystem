using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using MealService.Application.Specifications.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace MealService.Infrastructure.Implementations.Services
{
    public class CloudinaryService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string _defaultImageUrl;

        public CloudinaryService(IOptions<CloudinarySettings> config, IConfiguration configuration)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);

            var cloudName = config.Value.CloudName;
            _defaultImageUrl = configuration["DefaultImageUrl"]!.Replace("{CloudName}", cloudName);
        }

        public string GetDefaultImageUrl()
        {
            return _defaultImageUrl;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }

        public async Task DeleteImageAsync(string imagePublicId)
        {
            var deleteParams = new DeletionParams(imagePublicId);
            await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
