using System.Data;
using Board.Application.Exceptions;
using Board.Application.Ratings.Models.Commands;
using Board.Application.Repositories;
using MediatR;

namespace Board.Application.Ratings.CommandHandlers;

public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, int>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public UpdateRatingCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<int> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        var currentUser = await tenant.Users.GetById(request.CurrentUserId, cancellationToken);
        var bulletin = await tenant.Bulletins.GetById(request.BulletinId, cancellationToken);

        var rating = await tenant.Bulletins.Rating.TryGetRating(currentUser, bulletin, cancellationToken);

        if (rating is null)
        {
            throw NotFoundException.CreateForRating(currentUser.Id, bulletin.Id);
        }

        if (rating.RatingType == request.RatingType)
        {
            throw AlreadyExistsException.CreateByExistingRating(rating.UserId, rating.BulletinId, rating.RatingType);
        }

        await tenant.UnitOfWork.ExecuteInTransactionAsync(
            async () => await tenant.Bulletins.Rating.SwitchRatingType(rating, bulletin, cancellationToken),
            IsolationLevel.RepeatableRead,
            cancellationToken);

        return bulletin.Rating;
    }
}
