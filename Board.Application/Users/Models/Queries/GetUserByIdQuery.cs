using Board.Domain;
using MediatR;

namespace Board.Application.Users.Models.Queries;

public sealed record GetUserByIdQuery(Guid UserId): IRequest<User>;
