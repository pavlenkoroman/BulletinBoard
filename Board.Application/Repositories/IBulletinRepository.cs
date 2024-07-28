using Board.Application.Bulletins.Models.Queries;
using Board.Application.Models;
using Board.Domain;

namespace Board.Application.Repositories;

public interface IBulletinRepository
{
    IRatingRepository Rating { get; }
    Task Create(Bulletin bulletin, CancellationToken cancellationToken);
    Task<Bulletin> GetById(Guid bulletinId, CancellationToken cancellationToken);
    Task<Bulletin> GetByUserId(Guid currentUserId, Guid bulletinId, CancellationToken cancellationToken);
    Task<int> GetCountByUserId(Guid currentUserId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Bulletin>> GetExpired(CancellationToken cancellationToken);
    void Delete(Bulletin bulletin);
    Task<SearchResult<Bulletin>> Search(SearchBulletinQuery query, CancellationToken cancellationToken);
}
