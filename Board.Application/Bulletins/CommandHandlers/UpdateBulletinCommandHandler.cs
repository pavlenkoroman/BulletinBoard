using Board.Application.Bulletins.Models.Commands;
using Board.Application.Repositories;
using Board.Application.Services.Files.Services;
using MediatR;

namespace Board.Application.Bulletins.CommandHandlers;

public class UpdateBulletinCommandHandler : IRequestHandler<UpdateBulletinCommand, Guid>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IPhotoService _photoService;

    public UpdateBulletinCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IPhotoService photoService)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);
        ArgumentNullException.ThrowIfNull(photoService);

        _unitOfWorkFactory = unitOfWorkFactory;
        _photoService = photoService;
    }

    public async Task<Guid> Handle(UpdateBulletinCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var bulletin = await unitOfWork.Bulletins
            .GetByUserId(request.CurrentUserId, request.BulletinId, cancellationToken);

        if (request.Text is not null)
        {
            bulletin.UpdateText(request.Text);
        }

        var oldPhoto = bulletin.Photo;

        if (request.Image is not null)
        {
            var newPhoto = await _photoService.UploadFile(request.Image, cancellationToken);
            bulletin.UpdatePhoto(newPhoto);
        }

        try
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _photoService.DeleteFile(bulletin.Photo, cancellationToken);
            bulletin.UpdatePhoto(oldPhoto);
            throw;
        }

        if (request.Image is not null)
        {
            await _photoService.DeleteFile(oldPhoto, cancellationToken);
        }

        return bulletin.Id;
    }
}
