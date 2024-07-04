using Board.Application.Services.Files.Models;
using MediatR;

namespace Board.Application.Models.Bulletins.Commands;

public sealed record CreateBulletinCommand(Guid UserId, string Text, UploadFile File, DateTime ExpirationDate)
    : IRequest<Guid>;
