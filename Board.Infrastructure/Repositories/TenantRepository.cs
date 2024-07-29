using System.Data;
using Board.Application.Repositories;
using Board.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Board.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly DataContext _dbContext;

    public IUnitOfWork UnitOfWork { get; }
    public IUserRepository Users { get; }
    public IBulletinRepository Bulletins { get; }

    public TenantRepository(DataContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;

        UnitOfWork = dbContext;
        Users = new UserRepository(_dbContext);
        Bulletins = new BulletinRepository(_dbContext, new RatingRepository(_dbContext));
    }
}
