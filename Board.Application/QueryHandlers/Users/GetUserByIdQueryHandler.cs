using Board.Application.Models.Users.Queries;
using Board.Application.Repositories;
using Board.Domain;
using MediatR;

namespace Board.Application.QueryHandlers.Users;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetUserByIdQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        return await unitOfWork.Users.GetById(request.UserId, cancellationToken);
    }
}
