using MediatR;

namespace Board.Application.Ratings.Models.Commands;

public sealed record DeleteRatingCommand(Guid CurrentUserId, Guid BulletinId) : IRequest<int>;
