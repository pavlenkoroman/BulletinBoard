using System.Data;
using Board.Application.Repositories;
using Board.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Board.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dbContext;
    private IDbContextTransaction? _transaction;

    public IUserRepository Users { get; }
    public IBulletinRepository Bulletins { get; }

    public UnitOfWork(DataContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;

        Users = new UserRepository(_dbContext);
        Bulletins = new BulletinRepository(_dbContext, new RatingRepository(_dbContext));
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (_transaction is not null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    private async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task ExecuteInTransactionAsync(
        Func<Task> operation,
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await BeginTransactionAsync(isolationLevel, cancellationToken);

            try
            {
                await operation();
                await CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    private async Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
    {
        _transaction ??= await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }
}
