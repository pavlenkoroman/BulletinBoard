using Board.Application.Models.Bulletins.Queries;
using Board.Application.Models.Search;
using Board.Application.Repositories;
using Board.Domain;
using MediatR;

namespace Board.Application.QueryHandlers.Bulletins;

public sealed class SearchBulletinQueryHandler : IRequestHandler<SearchBulletinQuery, SearchResult<Bulletin>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public SearchBulletinQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<SearchResult<Bulletin>> Handle(SearchBulletinQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        return await unitOfWork.Bulletins.Search(request, cancellationToken);
    }
}
