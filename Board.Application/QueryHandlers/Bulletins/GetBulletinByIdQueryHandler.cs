using Board.Application.Models.Bulletins.Queries;
using Board.Application.Repositories;
using Board.Domain;
using MediatR;

namespace Board.Application.QueryHandlers.Bulletins;

public class GetBulletinByIdQueryHandler : IRequestHandler<GetBulletinByIdQuery, Bulletin>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetBulletinByIdQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Bulletin> Handle(GetBulletinByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        return await unitOfWork.Bulletins.GetById(request.BulletinId, cancellationToken);
    }
}
