namespace Board.Application.Services.Files.Models;

public class UploadFile
{
    public string Name { get; private init; }
    public string ContentType { get; private init; }
    public long Length { get; private init; }
    public Stream Stream { get; private init; }

    public UploadFile(string name, string contentType, long length, Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        if (name.Contains('\\', StringComparison.InvariantCultureIgnoreCase))
        {
            throw new InvalidOperationException("symbol '\\' is prohibited");
        }

        Name = name;
        ContentType = contentType;
        Length = length;
        Stream = stream;
    }
}
