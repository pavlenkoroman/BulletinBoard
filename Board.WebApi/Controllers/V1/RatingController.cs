﻿using Board.Application.Models.Rating.Commands;
using Board.Contract.Requests.Rating;
using Board.Contract.Responses;
using Board.Contract.Transfers;
using Board.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Board.WebApi.Controllers.V1;

[Route("api/v1/[controller]")]
public sealed class RatingController(IMediator mediator) : BaseV1Controller(mediator)
{
    [HttpPost("create")]
    public async Task<ActionResult<IdResponse>> Create(
        [FromBody] CreateRatingRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateRatingCommand(
            request.CurrentUserId,
            request.BulletinId,
            request.RatingType switch
            {
                RatingTypeTransfer.Increase => RatingType.Increase,
                RatingTypeTransfer.Decrease => RatingType.Decrease,
                _ => throw new ArgumentOutOfRangeException()
            });

        var ratingCount = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return new ObjectResult(ratingCount) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpPut("update")]
    public async Task<ActionResult<IdResponse>> Update(
        [FromBody] UpdateRatingRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new UpdateRatingCommand(
            request.CurrentUserId,
            request.BulletinId,
            request.RatingType switch
            {
                RatingTypeTransfer.Increase => RatingType.Increase,
                RatingTypeTransfer.Decrease => RatingType.Decrease,
                _ => throw new ArgumentOutOfRangeException()
            });

        var ratingCount = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

        return new ObjectResult(ratingCount) { StatusCode = StatusCodes.Status200OK };
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteRatingRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new DeleteRatingCommand(request.CurrentUserId, request.BulletinId);

        var ratingCount = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

        return new ObjectResult(ratingCount) { StatusCode = StatusCodes.Status200OK };
    }
}
