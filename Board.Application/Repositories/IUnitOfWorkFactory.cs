namespace Board.Application.Repositories;

public interface IUnitOfWorkFactory
{
    IUnitOfWork GetUnitOfWork();
}
