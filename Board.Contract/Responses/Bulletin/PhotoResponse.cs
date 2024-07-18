using System.Runtime.Serialization;

namespace Board.Contract.Responses.Bulletin;

[DataContract]
public record PhotoResponse(Uri OriginalUrl, Uri ResizedUrl);
