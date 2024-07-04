using Board.Application.Services.Files.Models;
using MediatR;

namespace Board.Application.Models.Bulletins.Commands;

public sealed record UpdateBulletinCommand(Guid BulletinId, Guid CurrentUserId, string? Text, UploadFile? Image)
    : IRequest<Guid>;
