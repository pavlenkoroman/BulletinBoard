using Board.Application.Exceptions;
using Board.Application.Models.Bulletins.Commands;
using Board.Application.Options;
using Board.Application.Repositories;
using Board.Application.Services.Files.Services;
using Board.Domain;
using MediatR;

namespace Board.Application.CommandHandlers.Bulletins;

public class CreateBulletinCommandHandler : IRequestHandler<CreateBulletinCommand, Guid>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IPhotoService _photoService;
    private readonly IBoardOption _boardOption;

    public CreateBulletinCommandHandler(
        IUnitOfWorkFactory unitOfWorkFactory,
        IPhotoService photoService,
        IBoardOption boardOption)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);
        ArgumentNullException.ThrowIfNull(photoService);
        ArgumentNullException.ThrowIfNull(boardOption);

        _unitOfWorkFactory = unitOfWorkFactory;
        _photoService = photoService;
        _boardOption = boardOption;
    }

    public async Task<Guid> Handle(CreateBulletinCommand request, CancellationToken cancellationToken)
    {
        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var filePath = await _photoService.UploadFile(request.File, cancellationToken);

        var currentUserBulletins = await unitOfWork.Bulletins.GetCountByUserId(request.UserId, cancellationToken);

        if (currentUserBulletins == _boardOption.MaxBulletinsPerUser)
        {
            throw LimitException.CreateByUserId(request.UserId);
        }

        var bulletin = Bulletin.Create(0, request.UserId, request.Text, filePath, _boardOption.BulletinsExpirationDays);

        await unitOfWork.Bulletins.Create(bulletin, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return bulletin.Id;
    }
}
