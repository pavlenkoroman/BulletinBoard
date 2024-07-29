using System.Data;
using Board.Application.Exceptions;
using Board.Application.Ratings.Models.Commands;
using Board.Application.Repositories;
using Board.Domain;
using MediatR;

namespace Board.Application.Ratings.CommandHandlers;

public sealed record CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, int>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public CreateRatingCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<int> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        var currentUser = await tenant.Users.GetById(request.CurrentUserId, cancellationToken);
        var bulletin = await tenant.Bulletins.GetById(request.BulletinId, cancellationToken);

        var rating = await tenant.Bulletins.Rating.TryGetRating(currentUser, bulletin, cancellationToken);

        if (rating is not null)
        {
            throw AlreadyExistsException.CreateByExistingRating(rating.UserId, rating.BulletinId, rating.RatingType);
        }

        rating = Rating.Create(currentUser, bulletin, request.RatingType);

        await tenant.UnitOfWork.ExecuteInTransactionAsync(
            async () => await tenant.Bulletins.Rating.Add(rating, bulletin, cancellationToken),
            IsolationLevel.RepeatableRead,
            cancellationToken);


        return bulletin.Rating;
    }
}
