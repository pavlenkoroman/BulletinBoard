using Board.Domain;
using MediatR;

namespace Board.Application.Ratings.Models.Commands;

public sealed record UpdateRatingCommand(Guid CurrentUserId, Guid BulletinId, RatingType RatingType) : IRequest<int>;
