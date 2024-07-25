using Board.Application.Models;
using Board.Application.Users.Models.Queries;
using Board.Domain;

namespace Board.Application.Repositories;

public interface IUserRepository
{
    Task Create(User user, CancellationToken cancellationToken);
    Task<User> GetById(Guid userId, CancellationToken cancellationToken);
    void Delete(User user);
    Task<SearchResult<User>> Search(SearchUserQuery query, CancellationToken cancellationToken);
}
