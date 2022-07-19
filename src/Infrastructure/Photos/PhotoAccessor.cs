namespace Carpool.Infrastructure.Photos;
public class PhotoAccessor : IPhotoAccessor
{
    private readonly Cloudinary _cloudinary;

    public async Task<PhotoUploadResult> AddPhoto(IFormFile file)
    {
        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill")
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new System.Exception(uploadResult.Error.Message);
            }
            return new PhotoUploadResult
            {
                PublicID = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString()
            };
        }
        return null!;
    }
    public PhotoAccessor(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret);
        _cloudinary = new Cloudinary(account);
    }

    
}
