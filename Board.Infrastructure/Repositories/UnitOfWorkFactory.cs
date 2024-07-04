using Board.Application.Repositories;
using Board.Infrastructure.Context;

namespace Board.Infrastructure.Repositories;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly DataContext _dbContext;
    private IUnitOfWork? _unitOfWork;

    public UnitOfWorkFactory(DataContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public IUnitOfWork GetUnitOfWork()
    {
        return _unitOfWork ??= new UnitOfWork(_dbContext);
    }
}
