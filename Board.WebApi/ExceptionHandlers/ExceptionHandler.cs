using System.Net.Mime;
using System.Text.Json;
using Board.WebApi.ExceptionHandlers.Factory;
using Microsoft.AspNetCore.Diagnostics;

namespace Board.WebApi.ExceptionHandlers;

public class ExceptionHandler
{
    private readonly IExceptionResponseBuilderFactory _factory;

    public ExceptionHandler(IExceptionResponseBuilderFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _factory = factory;
    }

    public Task Invoke(HttpContext? context)
    {
        try
        {
            if (context is null)
            {
                return Task.CompletedTask;
            }

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature?.Error is null)
            {
                return Task.CompletedTask;
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;

            var exception = contextFeature.Error;
            var builder = _factory.Create(exception);
            context.Response.StatusCode = (int)builder.StatusCode;
            var exceptionResponse = builder.Build(exception);

            var json = JsonSerializer.Serialize(exceptionResponse);
            return context.Response.WriteAsync(json);
        }
#pragma warning disable CA1031
        catch (Exception e)
#pragma warning restore CA1031
        {
            return Task.FromException(e);
        }
    }
}
