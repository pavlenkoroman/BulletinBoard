using System.Runtime.Serialization;

namespace Board.Contract.Requests.Bulletin;

[DataContract]
public sealed record UpdateBulletinRequest(Guid CurrentUserId, string? Text);
