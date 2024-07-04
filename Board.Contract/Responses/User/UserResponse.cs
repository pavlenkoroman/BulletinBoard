using System.Runtime.Serialization;

namespace Board.Contract.Responses.User;

[DataContract]
public sealed record UserResponse(Guid Id, string Name, bool IsAdmin);
