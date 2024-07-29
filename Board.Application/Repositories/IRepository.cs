namespace Board.Application.Repositories;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}
