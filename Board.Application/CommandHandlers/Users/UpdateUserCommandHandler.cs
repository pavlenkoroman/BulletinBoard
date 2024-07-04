using Board.Application.Models.Users.Commands;
using Board.Application.Repositories;
using MediatR;

namespace Board.Application.CommandHandlers.Users;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public UpdateUserCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var user = await unitOfWork.Users.GetById(request.UserId, cancellationToken);

        user.UpdateName(request.Name);

        await unitOfWork.CommitAsync(cancellationToken);

        return user.Id;
    }
}
