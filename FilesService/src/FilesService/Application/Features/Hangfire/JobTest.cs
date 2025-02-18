using FilesService.Application.Endpoints;
using FilesService.Application.Jobs;
using Hangfire;

namespace FilesService.Application.Features.Hangfire;

public static class JobTest
{
    public sealed class EndPoint : IEndpoint
    {

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("hangfire-job", Handler);
        }
    }


    private static async Task<IResult> Handler(CancellationToken ct)
    {
        var enqueueAt = TimeSpan.FromSeconds(new Random().Next(30));
        var jobId = BackgroundJob.Schedule<ConfirmConsistencyJob>(j => j.Execute(Guid.NewGuid(), "key", "bucket", ct), enqueueAt);
        return Results.Ok(jobId);
    }
}

