using System.Data;
using Board.Application.Models.Rating.Commands;
using Board.Application.Repositories;
using MediatR;

namespace Board.Application.CommandHandlers.Ratings;

public sealed class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, int>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public DeleteRatingCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<int> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var currentUser = await unitOfWork.Users.GetById(request.CurrentUserId, cancellationToken);
        var bulletin = await unitOfWork.Bulletins.GetById(request.BulletinId, cancellationToken);

        var rating = await unitOfWork.Bulletins.Rating.TryGetRating(currentUser, bulletin, cancellationToken);

        if (rating is null)
        {
            return bulletin.Rating;
        }

        await unitOfWork.ExecuteInTransactionAsync(
            async () => await unitOfWork.Bulletins.Rating.Remove(rating, bulletin, cancellationToken),
            IsolationLevel.RepeatableRead,
            cancellationToken);

        return bulletin.Rating;
    }
}
