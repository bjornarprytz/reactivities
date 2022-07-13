using Application.Interfaces;
using Application.Photos;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Photos;

public class PhotoAccessor : IPhotoAccessor
{
    private readonly Cloudinary _cloudinary;
    private readonly IOptions<CloudinarySettings> _config;

    public PhotoAccessor(IOptions<CloudinarySettings> config)
    {
        _config = config;
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<PhotoUploadResult?> AddPhotoAsync(IFormFile file, CancellationToken cancellationToken=default)
    {
        if (file.Length == 0) return default;

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (uploadResult.Error is not null)
        {
            throw new Exception(uploadResult.Error.Message);
        }

        return new PhotoUploadResult(uploadResult.PublicId, uploadResult.SecureUrl.ToString());

    }

    public async Task<string?> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(deleteParams);

        return result.Result == "ok" ? result.Result : null;
    }
}

