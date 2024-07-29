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

        await tenant.Bulletins.DeactivateExpired(cancellationTokenSource.Token);
    }
}
