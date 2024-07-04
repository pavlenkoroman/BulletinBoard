using Board.Application.Models.Search;
using Board.Application.Models.Users.Queries;
using Board.Application.Repositories;
using Board.Domain;
using MediatR;

namespace Board.Application.QueryHandlers.Users;

public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, SearchResult<User>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public SearchUserQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<SearchResult<User>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        return await unitOfWork.Users.Search(request, cancellationToken);
    }
}
