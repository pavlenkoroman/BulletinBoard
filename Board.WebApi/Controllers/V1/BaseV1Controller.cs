using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Board.WebApi.Controllers.V1;

[ApiController]
public abstract class BaseV1Controller : ControllerBase
{
    protected IMediator Mediator { get; }

    protected BaseV1Controller(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        Mediator = mediator;
    }
}
