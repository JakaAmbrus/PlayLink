using Microsoft.AspNetCore.Http;
using Social.Application.Models;

namespace Social.Application.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, string typeOfPhoto);
        Task<PhotoDeletionResult> DeletePhotoAsync(string publicId);
    }
}
