using Application.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoUploadResult> AddPhotoAsync(IFormFile file, string typeOfPhoto);
        Task<PhotoDeletionResult> DeletePhotoAsync(string publicId);
    }
}
