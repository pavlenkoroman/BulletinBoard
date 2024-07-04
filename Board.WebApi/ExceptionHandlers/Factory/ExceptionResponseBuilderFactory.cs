using System.Net;
using Board.WebApi.ExceptionHandlers.Builder;

namespace Board.WebApi.ExceptionHandlers.Factory;

internal class ExceptionResponseBuilderFactory(bool setStackTrace) : IExceptionResponseBuilderFactory
{
    public IExceptionResponseBuilder Create(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return new ExceptionResponseBuilder(setStackTrace, GetErrorCode(exception));
    }

    private static HttpStatusCode GetErrorCode(Exception exception)
    {
        return exception switch
        {
            NotImplementedException => HttpStatusCode.NotImplemented,
            ApplicationException => HttpStatusCode.BadRequest,
            ArgumentException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
    }
}
