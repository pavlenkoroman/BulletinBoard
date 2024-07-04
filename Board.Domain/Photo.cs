namespace Board.Domain;

public sealed class Photo
{
    public Guid Id { get; private init; }
    public string OriginalRelativePath { get; private init; }
    public Uri OriginalUrl { get; private init; }
    public string ResizedRelationalPath { get; private init; }
    public Uri ResizedUrl { get; private init; }

    public Photo(Guid id, string originalRelativePath, Uri originalUrl, string resizedRelationalPath, Uri resizedUrl)
    {
        Id = id;
        OriginalRelativePath = originalRelativePath;
        OriginalUrl = originalUrl;
        ResizedRelationalPath = resizedRelationalPath;
        ResizedUrl = resizedUrl;
    }

    public static Photo Create(
        string originalRelationalPath, 
        Uri originalUrl, 
        string resizedRelationalPath,
        Uri resizedUrl)
    {
        return new Photo(Guid.NewGuid(), originalRelationalPath, originalUrl, resizedRelationalPath, resizedUrl);
    }
}
