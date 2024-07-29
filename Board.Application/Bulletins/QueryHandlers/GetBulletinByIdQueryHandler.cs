using Board.Application.Bulletins.Models.Queries;
using Board.Application.Repositories;
using Board.Domain;
using MediatR;

namespace Board.Application.Bulletins.QueryHandlers;

public class GetBulletinByIdQueryHandler : IRequestHandler<GetBulletinByIdQuery, Bulletin>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetBulletinByIdQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Bulletin> Handle(GetBulletinByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        return await tenant.Bulletins.GetById(request.BulletinId, cancellationToken);
    }
}
