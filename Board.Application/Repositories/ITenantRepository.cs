using System.Data;

namespace Board.Application.Repositories;

public interface ITenantRepository
{
    public IUnitOfWork UnitOfWork { get; }
    IUserRepository Users { get; }
    IBulletinRepository Bulletins { get; }
}
