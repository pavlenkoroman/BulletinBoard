using MediatR;

namespace Board.Application.Users.Models.Commands;

public sealed record DeleteUserCommand(Guid UserId) : IRequest;
