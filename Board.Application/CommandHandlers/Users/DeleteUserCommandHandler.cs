using Board.Application.Models.Users.Commands;
using Board.Application.Repositories;
using MediatR;

namespace Board.Application.CommandHandlers.Users;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public DeleteUserCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var user = await unitOfWork.Users.GetById(request.UserId, cancellationToken);
        unitOfWork.Users.Delete(user);

        await unitOfWork.CommitAsync(cancellationToken);
    }
}
