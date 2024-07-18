﻿using Board.Application.Models.Bulletins;
using Board.Application.Models.Bulletins.Commands;
using Board.Application.Models.Bulletins.Queries;
using Board.Application.Models.Search;
using Board.Application.Services.Files.Models;
using Board.Contract.Requests.Bulletin;
using Board.Contract.Responses;
using Board.Contract.Responses.Bulletin;
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
            new UploadFile(photo.FileName, photo.ContentType, photo.Length, photo.OpenReadStream()),
            request.ExpirationDate);

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
                    bulletin.Photo.Id,
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
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var queryModel = new SearchBulletinQuery(
            new Page(page, pageSize),
            query,
            new IntegerRange(numberRangeStart, numberRangeEnd),
            userId,
            new IntegerRange(ratingRangeStart, ratingRangeEnd),
            new DateTimeRange(createdDateStart, createdDateEnd),
            new DateTimeRange(expirationDateStart, expirationDateEnd));

        var queryResult = await Mediator.Send(queryModel, cancellationToken).ConfigureAwait(false);

        var response = new SearchResponse<BulletinResponse>
        {
            CurrentPage = page,
            PageSize = pageSize,
            Results = queryResult.Results.Select(x =>
                    new BulletinResponse(
                        x.Id,
                        x.Number,
                        x.UserId,
                        x.Text,
                        new PhotoResponse(
                            x.Photo.Id,
                            x.Photo.OriginalUrl,
                            x.Photo.ResizedUrl),
                        x.Rating,
                        x.CreatedDate,
                        x.ExpirationDate))
                .ToArray()
        };

        return Ok(response);
    }
}