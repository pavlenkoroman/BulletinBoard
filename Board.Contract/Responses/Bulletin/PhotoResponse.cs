using System.Runtime.Serialization;

namespace Board.Contract.Responses.Bulletin;

[DataContract]
public record PhotoResponse(Guid Id, Uri OriginalUrl, Uri ResizedUrl);
