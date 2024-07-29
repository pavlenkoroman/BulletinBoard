using Board.Application.Bulletins.Models.Commands;
using Board.Application.Exceptions;
using Board.Application.Options;
using Board.Application.Repositories;
using Board.Application.Services.Files.Services;
using Board.Domain;
using Board.Domain.Exceptions;
using MediatR;

namespace Board.Application.Bulletins.CommandHandlers;

public class CreateBulletinCommandHandler : IRequestHandler<CreateBulletinCommand, Guid>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
    private readonly IPhotoService _photoService;
    private readonly IBoardOption _boardOption;

    public CreateBulletinCommandHandler(
        ITenantRepositoryFactory tenantRepositoryFactory,
        IPhotoService photoService,
        IBoardOption boardOption)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);
        ArgumentNullException.ThrowIfNull(photoService);
        ArgumentNullException.ThrowIfNull(boardOption);

        _tenantRepositoryFactory = tenantRepositoryFactory;
        _photoService = photoService;
        _boardOption = boardOption;
    }

    public async Task<Guid> Handle(CreateBulletinCommand request, CancellationToken cancellationToken)
    {
        var tenant = _tenantRepositoryFactory.GetTenant();

        var filePath = await _photoService.UploadFile(request.File, cancellationToken);

        var currentUserBulletins = await tenant.Bulletins.GetCountByUserId(request.UserId, cancellationToken);

        if (currentUserBulletins == _boardOption.MaxBulletinsPerUser)
        {
            throw LimitException.CreateByUserId(request.UserId);
        }

        var bulletin = Bulletin.Create(0, request.UserId, request.Text, filePath, _boardOption.BulletinsExpirationDays);

        await tenant.Bulletins.Create(bulletin, cancellationToken);
        await tenant.UnitOfWork.CommitAsync(cancellationToken);

        return bulletin.Id;
    }
}
