using Board.Application.Services.Files.Models;
using Board.Domain;

namespace Board.Application.Services.Files.Services;

public interface IPhotoService
{
    Task<Photo> UploadFile(UploadFile file, CancellationToken cancellationToken);
    Task DeleteFile(Photo photo, CancellationToken cancellationToken);
}
