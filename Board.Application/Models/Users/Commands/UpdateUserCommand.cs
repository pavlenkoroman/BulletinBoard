using MediatR;

namespace Board.Application.Models.Users.Commands;

public sealed record UpdateUserCommand(Guid UserId, string Name) : IRequest<Guid>;
