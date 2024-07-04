namespace Board.Infrastructure.Services.Files;

public interface IStaticPhotoServiceOption
{
    public Uri BaseUrl { get; }
    public string DefaultImageFolderName { get; }
    public int WidthToResize { get; }
    public int HeightToResize { get; }
}
