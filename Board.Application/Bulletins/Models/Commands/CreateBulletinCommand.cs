using Board.Application.Services.Files.Models;
using MediatR;

namespace Board.Application.Bulletins.Models.Commands;

public sealed record CreateBulletinCommand(Guid UserId, string Text, UploadFile File) : IRequest<Guid>;
