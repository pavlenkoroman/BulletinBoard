using Board.Application.Repositories;
using Quartz;

namespace Board.Infrastructure.Jobs;

public class BulletinExpirationJob : IJob
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public BulletinExpirationJob(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var tenant = _tenantRepositoryFactory.GetTenant();

        var bulletinsToDeactivate = await tenant.Bulletins.GetExpired(cancellationTokenSource.Token);

        foreach (var bulletin in bulletinsToDeactivate)
        {
            bulletin.UpdateIsActive(false);
        }

        await tenant.UnitOfWork.CommitAsync(cancellationTokenSource.Token);
    }
}
