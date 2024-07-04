using System.Net;
using Board.Contract.Responses;
using ApplicationException = Board.Application.Exceptions.ApplicationException;

namespace Board.WebApi.ExceptionHandlers.Builder;

internal sealed class ExceptionResponseBuilder : IExceptionResponseBuilder
{
    private bool SetStackTrace { get; }

    internal ExceptionResponseBuilder(bool setStackTrace, HttpStatusCode statusCode)
    {
        SetStackTrace = setStackTrace;
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }

    public ExceptionResponse Build(Exception exception)
    {
        var exceptionType = exception.GetType();

        var details = exception switch
        {
            ApplicationException applicationException => applicationException.Errors?.Select(error => error.Code),
            _ => null
        };

        return new ExceptionResponse(exceptionType, exception.Message)
        {
            StackTrace = SetStackTrace ? exception.StackTrace : null,
            Details = details
        };
    }
}
