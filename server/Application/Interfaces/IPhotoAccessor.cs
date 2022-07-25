using Application.Photos;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IPhotoAccessor
{
    Task<PhotoUploadResult?> AddPhotoAsync(IFormFile file, CancellationToken cancellationToken=default);
    Task<string?> DeletePhotoAsync(string publicId);
}