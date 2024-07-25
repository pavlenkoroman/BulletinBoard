using Board.Domain;
using MediatR;

namespace Board.Application.Bulletins.Models.Queries;

public sealed record GetBulletinByIdQuery(Guid BulletinId) : IRequest<Bulletin>;
