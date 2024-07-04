using System.Runtime.Serialization;

namespace Board.Contract.Requests.Bulletin;

[DataContract]
public sealed record DeleteBulletinRequest(Guid CurrentUserId);
