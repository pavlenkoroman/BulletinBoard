using Board.Application.Bulletins.Models.Queries;
using Board.Application.Models;
using Board.Application.Repositories;
using Board.Domain;
using MediatR;

namespace Board.Application.Bulletins.QueryHandlers;

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
