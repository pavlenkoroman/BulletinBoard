using System.Runtime.Serialization;

namespace Board.Contract.Requests.User;

[DataContract]
public record UpdateUserRequest(string Name);
