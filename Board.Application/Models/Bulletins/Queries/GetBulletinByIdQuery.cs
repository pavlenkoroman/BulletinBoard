using Board.Domain;
using MediatR;

namespace Board.Application.Models.Bulletins.Queries;

public sealed record GetBulletinByIdQuery(Guid BulletinId) : IRequest<Bulletin>;
