using System.Runtime.Serialization;

namespace Board.Contract.Requests.Bulletin;

[DataContract]
public sealed record CreateBulletinRequest(Guid UserId, string Text, DateTime ExpirationDate);
