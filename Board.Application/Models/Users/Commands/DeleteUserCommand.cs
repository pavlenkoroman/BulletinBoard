using MediatR;

namespace Board.Application.Models.Users.Commands;

public sealed record DeleteUserCommand(Guid UserId) : IRequest;
