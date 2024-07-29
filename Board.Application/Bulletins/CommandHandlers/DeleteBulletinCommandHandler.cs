using Board.Application.Bulletins.Models.Commands;
using Board.Application.Repositories;
using Board.Application.Services.Files.Services;
using MediatR;

namespace Board.Application.Bulletins.CommandHandlers;

public sealed class DeleteBulletinCommandHandler : IRequestHandler<DeleteBulletinCommand>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
    private readonly IPhotoService _photoService;

    public DeleteBulletinCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory, IPhotoService photoService)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);
        ArgumentNullException.ThrowIfNull(photoService);

        _tenantRepositoryFactory = tenantRepositoryFactory;
        _photoService = photoService;
    }

    public async Task Handle(DeleteBulletinCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        var currentUserBulletin = await tenant.Bulletins.GetByUserId(
            request.CurrentUserId,
            request.BulletinId,
            cancellationToken);

        var photo = currentUserBulletin.Photo;

        tenant.Bulletins.Delete(currentUserBulletin);

        await tenant.UnitOfWork.CommitAsync(cancellationToken);

        await _photoService.DeleteFile(photo, cancellationToken);
    }
}
