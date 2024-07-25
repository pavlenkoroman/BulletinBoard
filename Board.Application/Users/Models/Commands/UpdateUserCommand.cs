using MediatR;

namespace Board.Application.Users.Models.Commands;

public sealed record UpdateUserCommand(Guid UserId, string Name) : IRequest<Guid>;
