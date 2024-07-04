using System.Runtime.Serialization;

namespace Board.Contract.Responses;

[DataContract]
public sealed record IdResponse(Guid Id);
