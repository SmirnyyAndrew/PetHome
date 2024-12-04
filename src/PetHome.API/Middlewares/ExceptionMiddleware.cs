using PetHome.API.Response;
using PetHome.Domain.Shared.Error;

namespace PetHome.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _nextMiddleware;

    public ExceptionMiddleware(RequestDelegate nextMiddleware)
    {
        _nextMiddleware = nextMiddleware;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _nextMiddleware(context);
        }
        catch (Exception ex)
        {
            Error error = Error.Failure(
                StatusCodes.Status500InternalServerError.ToString(), "server.is.internal", ex.Message);

            var envelope = ResponseEnvelope.Error(error);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}

