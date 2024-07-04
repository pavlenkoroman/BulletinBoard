using MediatR;

namespace Board.Application.Models.Bulletins.Commands;

public sealed record DeleteBulletinCommand(Guid CurrentUserId, Guid BulletinId) : IRequest;
