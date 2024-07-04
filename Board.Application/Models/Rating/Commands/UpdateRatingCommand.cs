using Board.Domain;
using MediatR;

namespace Board.Application.Models.Rating.Commands;

public sealed record UpdateRatingCommand(Guid CurrentUserId, Guid BulletinId, RatingType RatingType) : IRequest<int>;
