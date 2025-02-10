using FilesService.Core.ErrorManagment;
using FilesService.Core.ErrorManagment.Envelopes;

namespace FilesService.Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _nextMiddleware;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate nextMiddleware, ILogger<ExceptionMiddleware> logger)
    {
        _nextMiddleware = nextMiddleware;
        _logger = logger;
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

            _logger.LogError("------> " + ex, ex.Message);
        }
    }
}

