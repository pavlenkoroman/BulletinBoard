using Board.Application.Repositories;
using Quartz;

namespace Board.Infrastructure.Jobs;

public class BulletinExpirationJob : IJob
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public BulletinExpirationJob(IUnitOfWorkFactory unitOfWorkFactory)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var bulletinsToDeactivate = await unitOfWork.Bulletins.GetExpired(cancellationTokenSource.Token);

        foreach (var bulletin in bulletinsToDeactivate)
        {
            bulletin.UpdateIsActive(false);
        }

        await unitOfWork.CommitAsync(cancellationTokenSource.Token);
    }
}
