using Board.Application.Repositories;
using Board.Application.Users.Models.Commands;
using MediatR;

namespace Board.Application.Users.CommandHandlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public DeleteUserCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        var user = await tenant.Users.GetById(request.UserId, cancellationToken);
        tenant.Users.Delete(user);

        await tenant.UnitOfWork.CommitAsync(cancellationToken);
    }
}
