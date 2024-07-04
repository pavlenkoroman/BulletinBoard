using System.Net;
using Board.Contract.Responses;

namespace Board.WebApi.ExceptionHandlers.Builder;

public interface IExceptionResponseBuilder
{
    HttpStatusCode StatusCode { get; }

    ExceptionResponse Build(Exception exception);
}
