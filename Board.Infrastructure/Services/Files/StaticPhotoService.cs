using Board.Application.Services.Files.Models;
using Board.Application.Services.Files.Services;
using Board.Domain;
using ImageMagick;

namespace Board.Infrastructure.Services.Files;

public sealed class StaticPhotoService : IPhotoService
{
    private readonly IStaticPhotoServiceOption _options;

    public StaticPhotoService(IStaticPhotoServiceOption options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;
    }

    public async Task<Photo> UploadFile(UploadFile file, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(file);

        var extension = Path.GetExtension(file.Name);
        var physicalName = $"{file.Name[..^extension.Length]}_{Guid.NewGuid()}{extension}";
        var relativePath = BuildOriginalRelativePath(physicalName);
        var absolutePathToFile = BuildAbsolutePath(relativePath);

        await using var stream = new FileStream(relativePath, FileMode.Create);
        await file.Stream.CopyToAsync(stream, cancellationToken);
        file.Stream.Position = 0;

        var resized = UploadResizedImage(file);

        return Photo.Create(relativePath, absolutePathToFile, resized.relativePath, resized.absolutePath);
    }

    public Task DeleteFile(Photo photo, CancellationToken cancellationToken)
    {
        if (File.Exists(photo.OriginalRelativePath))
        {
            File.Delete(photo.OriginalRelativePath);
        }

        if (File.Exists(photo.ResizedRelationalPath))
        {
            File.Delete(photo.ResizedRelationalPath);
        }

        return Task.CompletedTask;
    }

    private (string relativePath, Uri absolutePath) UploadResizedImage(UploadFile file)
    {
        using var image = new MagickImage(file.Stream);

        if (image.Width >= _options.WidthToResize)
        {
            image.Resize(_options.WidthToResize, _options.HeightToResize);
        }

        var extension = Path.GetExtension(file.Name);
        var physicalName = $"{file.Name[..^extension.Length]}_{Guid.NewGuid()}{extension}";
        var relativePath = BuildResizedRelativePath(physicalName);
        var absolutePath = BuildAbsolutePath(relativePath);

        using var stream = new FileStream(relativePath, FileMode.Create);
        image.Write(stream);

        return (relativePath, absolutePath);
    }

    private string BuildOriginalRelativePath(string physicalName)
    {
        return $"{_options.DefaultImageFolderName}/Original/{physicalName}";
    }

    private string BuildResizedRelativePath(string physicalName)
    {
        return $"{_options.DefaultImageFolderName}/Resized/{physicalName}";
    }

    private Uri BuildAbsolutePath(string relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

        return new Uri(
            _options.BaseUrl.ToString().EndsWith('/') || relativePath.StartsWith('/')
                ? $"{_options.BaseUrl}{relativePath}"
                : $"{_options.BaseUrl}/{relativePath}");
    }
}
