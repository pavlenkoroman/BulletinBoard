using System.Data;
using System.Reflection;
using Board.Application.Repositories;
using Board.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Board.Infrastructure.Context;

public class DataContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    
    protected DataContext()
    {
    }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresEnum<RatingType>();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);

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
        var strategy = Database.CreateExecutionStrategy();

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
        _transaction ??= await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }
}
