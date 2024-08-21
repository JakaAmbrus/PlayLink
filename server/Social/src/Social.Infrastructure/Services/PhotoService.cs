using Social.Application.Interfaces;
using Social.Application.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Social.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Social.Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, string typeOfPhoto)
        {
            // Checks if the cloudinary account is configured
            if (string.IsNullOrEmpty(_cloudinary.Api.Account.Cloud) ||
                string.IsNullOrEmpty(_cloudinary.Api.Account.ApiKey) ||
                string.IsNullOrEmpty(_cloudinary.Api.Account.ApiSecret))
            {
                throw new BadRequestException("Cloudinary account not configured");
            }

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream(); //using so it disposes it from memory

                Transformation transformation = new();

                //different photo configurations for different types of services
                if (typeOfPhoto == "profile")
                {
                    transformation = new Transformation()
                        .Height(400)
                        .Width(400)
                        .Crop("fill")
                        .Gravity("face")
                        .Quality("auto:best")
                        .FetchFormat("auto");
                }
                else if (typeOfPhoto == "post")
                {
                    transformation = new Transformation()
                        .Height(600)
                        .Width(600)
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
                Url = uploadResult.SecureUrl.ToString(),
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
