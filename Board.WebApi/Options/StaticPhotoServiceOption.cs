using Board.Infrastructure.Services.Files;

namespace Board.WebApi.Options;

public class StaticPhotoServiceOption : IStaticPhotoServiceOption
{
    public const string Name = "StaticPhotos";
    public Uri BaseUrl { get; init; } = null!;
    public string DefaultImageFolderName { get; init; } = null!;
    public int WidthToResize { get; init; }
    public int HeightToResize { get; init; }
}
