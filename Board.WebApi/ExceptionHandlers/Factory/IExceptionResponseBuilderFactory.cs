using Board.WebApi.ExceptionHandlers.Builder;

namespace Board.WebApi.ExceptionHandlers.Factory;

public interface IExceptionResponseBuilderFactory
{
    IExceptionResponseBuilder Create(Exception exception);
}
