using Board.Application.Repositories;
using Board.Application.Users.Models.Commands;
using MediatR;

namespace Board.Application.Users.CommandHandlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public UpdateUserCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        var user = await tenant.Users.GetById(request.UserId, cancellationToken);

        user.UpdateName(request.Name);

        await tenant.UnitOfWork.CommitAsync(cancellationToken);

        return user.Id;
    }
}
