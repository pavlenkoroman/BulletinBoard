using MediatR;

namespace Board.Application.Bulletins.Models.Commands;

public sealed record DeleteBulletinCommand(Guid CurrentUserId, Guid BulletinId) : IRequest;
