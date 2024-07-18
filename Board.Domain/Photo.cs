namespace Board.Domain;

public sealed class Photo
{
    public string OriginalRelativePath { get; private init; }
    public Uri OriginalUrl { get; private init; }
    public string ResizedRelationalPath { get; private init; }
    public Uri ResizedUrl { get; private init; }

    public Photo(string originalRelativePath, Uri originalUrl, string resizedRelationalPath, Uri resizedUrl)
    {
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
        return new Photo(originalRelationalPath, originalUrl, resizedRelationalPath, resizedUrl);
    }
}
