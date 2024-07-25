using Board.Application.Bulletins.Models;
using Board.Application.Bulletins.Models.Commands;
using Board.Application.Bulletins.Models.Queries;
using Board.Application.Models;
using Board.Application.Services.Files.Models;
using Board.Contract.Requests.Bulletin;
using Board.Contract.Responses;
using Board.Contract.Responses.Bulletin;
using Board.Contract.Transfers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Board.WebApi.Controllers.V1;

[Route("api/v1/[controller]")]
public sealed class BulletinsController(IMediator mediator) : BaseV1Controller(mediator)
{
    [HttpPost]
    public async Task<ActionResult<IdResponse>> Create(
        [FromForm] CreateBulletinRequest request,
        IFormFile photo,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateBulletinCommand(
            request.UserId,
            request.Text,
            new UploadFile(photo.FileName, photo.ContentType, photo.Length, photo.OpenReadStream()));

        var userId = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return new ObjectResult(new IdResponse(userId)) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<IdResponse>> Update(
        Guid id,
        [FromForm] UpdateBulletinRequest request,
        IFormFile? photo,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateBulletinCommand(
            id,
            request.CurrentUserId,
            request.Text,
            photo is not null
                ? new UploadFile(photo.FileName, photo.ContentType, photo.Length, photo.OpenReadStream())
                : null);

        var userId = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return new ObjectResult(new IdResponse(userId)) { StatusCode = StatusCodes.Status200OK };
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromBody] DeleteBulletinRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new DeleteBulletinCommand(request.CurrentUserId, id);
        await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BulletinResponse>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetBulletinByIdQuery(id);
        var bulletin = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);

        return new ObjectResult(
            new BulletinResponse(
                bulletin.Id,
                bulletin.Number,
                bulletin.UserId,
                bulletin.Text,
                new PhotoResponse(
                    bulletin.Photo.OriginalUrl,
                    bulletin.Photo.ResizedUrl),
                bulletin.Rating,
                bulletin.CreatedDate,
                bulletin.ExpirationDate))
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpGet("search")]
    public async Task<ActionResult<SearchResponse<BulletinResponse>>> Get(
        [FromQuery] string? query,
        [FromQuery] int? numberRangeStart,
        [FromQuery] int? numberRangeEnd,
        [FromQuery] Guid? userId,
        [FromQuery] int? ratingRangeStart,
        [FromQuery] int? ratingRangeEnd,
        [FromQuery] DateTime? createdDateStart,
        [FromQuery] DateTime? createdDateEnd,
        [FromQuery] DateTime? expirationDateStart,
        [FromQuery] DateTime? expirationDateEnd,
        [FromQuery] BulletinSortByTransfer? sortBy,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var queryModel = new SearchBulletinQuery(
            new Page(page, pageSize),
            query,
            new Range<int>(numberRangeStart, numberRangeEnd),
            userId,
            new Range<int>(ratingRangeStart, ratingRangeEnd),
            new Range<DateTime>(createdDateStart, createdDateEnd),
            new Range<DateTime>(expirationDateStart, expirationDateEnd),
            sortBy switch {
                BulletinSortByTransfer.Number => BulletinSortBy.Number,
                BulletinSortByTransfer.Text => BulletinSortBy.Text,
                BulletinSortByTransfer.Rating => BulletinSortBy.Rating,
                BulletinSortByTransfer.CreatedDateAscending => BulletinSortBy.CreatedDateAscending,
                BulletinSortByTransfer.ExpirationDateAscending => BulletinSortBy.ExpirationDateAscending,
                BulletinSortByTransfer.CreatedDateDescending => BulletinSortBy.CreatedDateDescending,
                BulletinSortByTransfer.ExpirationDateDescending => BulletinSortBy.ExpirationDateDescending,
                null => null,
                _ => throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, "Unexpected enum value")
            });

        var queryResult = await Mediator.Send(queryModel, cancellationToken).ConfigureAwait(false);

        var response = new SearchResponse<BulletinResponse>
        {
            Results = queryResult.Results.Select(x =>
                    new BulletinResponse(
                        x.Id,
                        x.Number,
                        x.UserId,
                        x.Text,
                        new PhotoResponse(
                            x.Photo.OriginalUrl,
                            x.Photo.ResizedUrl),
                        x.Rating,
                        x.CreatedDate,
                        x.ExpirationDate))
                .ToArray(),
            Count = queryResult.Count
        };

        return Ok(response);
    }
}
