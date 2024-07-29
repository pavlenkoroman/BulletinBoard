using System.Data;
using Board.Application.Ratings.Models.Commands;
using Board.Application.Repositories;
using MediatR;

namespace Board.Application.Ratings.CommandHandlers;

public sealed class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, int>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public DeleteRatingCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<int> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        var currentUser = await tenant.Users.GetById(request.CurrentUserId, cancellationToken);
        var bulletin = await tenant.Bulletins.GetById(request.BulletinId, cancellationToken);

        var rating = await tenant.Bulletins.Rating.TryGetRating(currentUser, bulletin, cancellationToken);

        if (rating is null)
        {
            return bulletin.Rating;
        }

        await tenant.UnitOfWork.ExecuteInTransactionAsync(
            async () => await tenant.Bulletins.Rating.Remove(rating, bulletin, cancellationToken),
            IsolationLevel.RepeatableRead,
            cancellationToken);

        return bulletin.Rating;
    }
}
