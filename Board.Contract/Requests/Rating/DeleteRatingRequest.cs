using System.Runtime.Serialization;

namespace Board.Contract.Requests.Rating;

[DataContract]
public sealed record DeleteRatingRequest(Guid CurrentUserId, Guid BulletinId);
