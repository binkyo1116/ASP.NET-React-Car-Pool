

namespace Carpool.Application.Common.Interfaces;
public interface IPhotoAccessor
{
    Task<PhotoUploadResult> AddPhoto(IFormFile file);
}
