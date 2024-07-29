using Board.Application.Bulletins.Models.Commands;
using Board.Application.Repositories;
using Board.Application.Services.Files.Services;
using MediatR;

namespace Board.Application.Bulletins.CommandHandlers;

public class UpdateBulletinCommandHandler : IRequestHandler<UpdateBulletinCommand, Guid>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
    private readonly IPhotoService _photoService;

    public UpdateBulletinCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory, IPhotoService photoService)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);
        ArgumentNullException.ThrowIfNull(photoService);

        _tenantRepositoryFactory = tenantRepositoryFactory;
        _photoService = photoService;
    }

    public async Task<Guid> Handle(UpdateBulletinCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        var bulletin = await tenant.Bulletins
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
            await tenant.UnitOfWork.CommitAsync(cancellationToken);
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
