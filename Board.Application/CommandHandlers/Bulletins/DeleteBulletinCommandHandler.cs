using Board.Application.Models.Bulletins.Commands;
using Board.Application.Repositories;
using Board.Application.Services.Files.Services;
using MediatR;

namespace Board.Application.CommandHandlers.Bulletins;

public sealed class DeleteBulletinCommandHandler : IRequestHandler<DeleteBulletinCommand>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IPhotoService _photoService;

    public DeleteBulletinCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IPhotoService photoService)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);
        ArgumentNullException.ThrowIfNull(photoService);

        _unitOfWorkFactory = unitOfWorkFactory;
        _photoService = photoService;
    }

    public async Task Handle(DeleteBulletinCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var currentUserBulletin = await unitOfWork.Bulletins.GetByUserId(
            request.CurrentUserId,
            request.BulletinId,
            cancellationToken);

        var photo = currentUserBulletin.Photo;

        unitOfWork.Bulletins.Delete(currentUserBulletin);

        await unitOfWork.CommitAsync(cancellationToken);

        await _photoService.DeleteFile(photo, cancellationToken);
    }
}
