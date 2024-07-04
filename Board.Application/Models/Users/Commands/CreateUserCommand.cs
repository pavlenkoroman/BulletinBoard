using MediatR;

namespace Board.Application.Models.Users.Commands;

public sealed record CreateUserCommand(string Name, bool IsAdmin) : IRequest<Guid>;
