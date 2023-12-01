using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Application.Interfaces;
using Application.Models;

namespace Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
            _cloudinary.Api.Secure = true;
        }

        public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, string typeOfPhoto)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream(); //using so it disposes it from memory

                Transformation transformation = new();

                //different photo configurations for different types of services
                if (typeOfPhoto == "profile")
                {
                    transformation = new Transformation()
                        .Height(500)
                        .Width(500)
                        .Crop("fill")
                        .Gravity("face")
                        .Quality("auto:best")
                        .FetchFormat("auto");
                }
                else if (typeOfPhoto == "post")
                {
                    transformation = new Transformation()
                        .Height(800)
                        .Width(800)
                        .Crop("fill")
                        .Gravity("face")
                        .Quality("auto:best")
                        .FetchFormat("auto");
                }

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = transformation,
                    Folder = "playlink-images"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            var result = new PhotoUploadResult
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.Url.ToString(),
                Error = uploadResult.Error != null ? uploadResult.Error.Message : null
            };

            return result;
        }

        public async Task<PhotoDeletionResult> DeletePhotoAsync(string publicId)
        {

            var deletionResult = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

            return new PhotoDeletionResult
            {
                Succeeded = deletionResult.Result == "ok",
                Error = deletionResult.Error != null ? deletionResult.Error.Message : null
            };
        }

    }
}
