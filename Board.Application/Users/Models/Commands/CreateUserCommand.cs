using MediatR;

namespace Board.Application.Users.Models.Commands;

public sealed record CreateUserCommand(string Name, bool IsAdmin) : IRequest<Guid>;
