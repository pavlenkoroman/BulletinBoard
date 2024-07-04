using System.Data;

namespace Board.Application.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IBulletinRepository Bulletins { get; }
    Task CommitAsync(CancellationToken cancellationToken);
    Task ExecuteInTransactionAsync(
        Func<Task> operation,
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken);
}
