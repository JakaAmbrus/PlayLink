using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file, string typeOfPhoto);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
