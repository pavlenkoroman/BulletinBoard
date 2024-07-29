using Board.Application.Repositories;
using Board.Application.Users.Models.Queries;
using Board.Domain;
using MediatR;

namespace Board.Application.Users.QueryHandlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetUserByIdQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        return await tenant.Users.GetById(request.UserId, cancellationToken);
    }
}
