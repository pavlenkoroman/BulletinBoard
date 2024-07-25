using Board.Application.Services.Files.Models;
using MediatR;

namespace Board.Application.Bulletins.Models.Commands;

public sealed record UpdateBulletinCommand(Guid BulletinId, Guid CurrentUserId, string? Text, UploadFile? Image)
    : IRequest<Guid>;
