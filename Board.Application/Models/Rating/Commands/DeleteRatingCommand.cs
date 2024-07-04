using MediatR;

namespace Board.Application.Models.Rating.Commands;

public sealed record DeleteRatingCommand(Guid CurrentUserId, Guid BulletinId) : IRequest<int>;
