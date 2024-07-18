using Board.Application.Models.Search;
using Board.Application.Models.Users;
using Board.Application.Models.Users.Commands;
using Board.Application.Models.Users.Queries;
using Board.Contract.Requests.User;
using Board.Contract.Responses;
using Board.Contract.Responses.User;
using Board.Contract.Transfers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Board.WebApi.Controllers.V1;

[Route("api/v1/[controller]")]
public class UsersController(IMediator mediator) : BaseV1Controller(mediator)
{
    [HttpPost]
    public async Task<ActionResult<IdResponse>> Create(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateUserCommand(request.Name, request.IsAdmin);
        var userId = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return new ObjectResult(new IdResponse(userId)) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<IdResponse>> Update(
        Guid id,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateUserCommand(id, request.Name);
        var userId = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return new ObjectResult(new IdResponse(userId)) { StatusCode = StatusCodes.Status200OK };
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteUserCommand(id);
        await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByIdQuery(id);
        var user = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);

        return new ObjectResult(new UserResponse(user.Id, user.Name, user.IsAdmin))
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpGet("search")]
    public async Task<ActionResult<SearchResponse<UserResponse>>> Get(
        [FromQuery] string? query,
        [FromQuery] bool? isAdmin,
        [FromQuery] UserSortByTransfer? sortBy,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var queryModel = new SearchUserQuery(
            new Page(page, pageSize),
            query,
            isAdmin,
            sortBy switch
            {
                UserSortByTransfer.Name => UserSortBy.Name,
                UserSortByTransfer.IsAdmin => UserSortBy.IsAdmin,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, "Unexpected enum value")
            });

        var searchResult = await Mediator.Send(queryModel, cancellationToken).ConfigureAwait(false);

        var response = new SearchResponse<UserResponse>
        {
            Results = searchResult.Results.Select(x => new UserResponse(x.Id, x.Name, x.IsAdmin)).ToList(),
            Count = searchResult.Count
        };

        return Ok(response);
    }
}
