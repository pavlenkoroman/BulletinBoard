using Board.Application.Repositories;
using Board.Infrastructure.Context;

namespace Board.Infrastructure.Repositories;

public class TenantRepositoryFactory : ITenantRepositoryFactory
{
    private readonly DataContext _dbContext;
    private ITenantRepository? _unitOfWork;

    public TenantRepositoryFactory(DataContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public ITenantRepository GetTenant()
    {
        return _unitOfWork ??= new TenantRepository(_dbContext);
    }
}
