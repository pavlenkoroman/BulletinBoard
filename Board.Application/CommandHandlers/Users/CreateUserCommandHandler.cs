using Board.Application.Models.Users.Commands;
using Board.Application.Repositories;
using Board.Domain;
using MediatR;

namespace Board.Application.CommandHandlers.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public CreateUserCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        ArgumentNullException.ThrowIfNull(unitOfWorkFactory);

        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unitOfWork = _unitOfWorkFactory.GetUnitOfWork();

        var user = User.Create(request.Name, request.IsAdmin);

        await unitOfWork.Users.Create(user, cancellationToken).ConfigureAwait(false);

        await unitOfWork.CommitAsync(cancellationToken);

        return user.Id;
    }
}
