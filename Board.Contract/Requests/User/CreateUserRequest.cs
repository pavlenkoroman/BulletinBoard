using System.Runtime.Serialization;

namespace Board.Contract.Requests.User;

[DataContract]
public sealed record CreateUserRequest(string Name, bool IsAdmin);
