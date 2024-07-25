using System.Data;
using Board.Application.Exceptions;
using Board.Application.Ratings.Models.Commands;
using Board.Application.Repositories;
using MediatR;

namespace Board.Application.Ratings.CommandHandlers;

public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, int>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public UpdateRatingCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<int> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var currentUser = await unitOfWork.Users.GetById(request.CurrentUserId, cancellationToken);
        var bulletin = await unitOfWork.Bulletins.GetById(request.BulletinId, cancellationToken);

        var rating = await unitOfWork.Bulletins.Rating.TryGetRating(currentUser, bulletin, cancellationToken);

        if (rating is null)
        {
            throw NotFoundException.CreateForRating(currentUser.Id, bulletin.Id);
        }

        if (rating.RatingType == request.RatingType)
        {
            throw AlreadyExistsException.CreateByExistingRating(rating.UserId, rating.BulletinId, rating.RatingType);
        }

        await unitOfWork.ExecuteInTransactionAsync(
            async () => await unitOfWork.Bulletins.Rating.SwitchRatingType(rating, bulletin, cancellationToken),
            IsolationLevel.RepeatableRead,
            cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        return bulletin.Rating;
    }
}
