using Board.Domain;
using MediatR;

namespace Board.Application.Models.Users.Queries;

public sealed record GetUserByIdQuery(Guid UserId): IRequest<User>;
