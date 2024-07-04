using System.Runtime.Serialization;
using Board.Contract.Transfers;

namespace Board.Contract.Requests.Rating;

[DataContract]
public sealed record CreateRatingRequest(Guid CurrentUserId, Guid BulletinId, RatingTypeTransfer RatingType);
