using System.Runtime.Serialization;

namespace Board.Contract.Responses.Bulletin;

[DataContract]
public sealed record BulletinResponse(
    Guid Id,
    int Number,
    Guid UserId,
    string Text,
    PhotoResponse Photo,
    int Rating,
    DateTime CreatedDate,
    DateTime ExpirationDate);
