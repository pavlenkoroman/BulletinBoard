using Board.Domain;
using MediatR;

namespace Board.Application.Models.Rating.Commands;

public sealed record CreateRatingCommand(Guid CurrentUserId, Guid BulletinId, RatingType RatingType)
    : IRequest<int>;
