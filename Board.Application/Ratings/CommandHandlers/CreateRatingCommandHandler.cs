using System.Data;
using Board.Application.Exceptions;
using Board.Application.Ratings.Models.Commands;
using Board.Application.Repositories;
using Board.Domain;
using MediatR;

namespace Board.Application.Ratings.CommandHandlers;

public sealed record CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, int>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public CreateRatingCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<int> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var currentUser = await unitOfWork.Users.GetById(request.CurrentUserId, cancellationToken);
        var bulletin = await unitOfWork.Bulletins.GetById(request.BulletinId, cancellationToken);

        var rating = await unitOfWork.Bulletins.Rating.TryGetRating(currentUser, bulletin, cancellationToken);

        if (rating is not null)
        {
            throw AlreadyExistsException.CreateByExistingRating(rating.UserId, rating.BulletinId, rating.RatingType);
        }

        rating = Rating.Create(currentUser, bulletin, request.RatingType);

        await unitOfWork.ExecuteInTransactionAsync(
            async () => await unitOfWork.Bulletins.Rating.Add(rating, bulletin, cancellationToken),
            IsolationLevel.RepeatableRead,
            cancellationToken);


        return bulletin.Rating;
    }
}
