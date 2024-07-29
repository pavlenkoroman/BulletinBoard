using Board.Application.Repositories;
using Board.Application.Users.Models.Commands;
using Board.Domain;
using MediatR;

namespace Board.Application.Users.CommandHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public CreateUserCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        var user = User.Create(request.Name, request.IsAdmin);

        await tenant.Users.Create(user, cancellationToken).ConfigureAwait(false);

        await tenant.UnitOfWork.CommitAsync(cancellationToken);

        return user.Id;
    }
}
