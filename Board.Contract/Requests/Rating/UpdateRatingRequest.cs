using System.Runtime.Serialization;
using Board.Contract.Transfers;

namespace Board.Contract.Requests.Rating;

[DataContract]
public sealed record UpdateRatingRequest(Guid CurrentUserId, Guid BulletinId, RatingTypeTransfer RatingType);
