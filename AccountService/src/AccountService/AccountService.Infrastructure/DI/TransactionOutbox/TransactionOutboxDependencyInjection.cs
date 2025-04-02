using AccountService.Infrastructure.TransactionOutbox;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AccountService.Infrastructure.DI.TransactionOutbox;
public static class TransactionOutboxDependencyInjection
{
    public static IServiceCollection AddOutboxService(this IServiceCollection services)
    {
        services.AddScoped<ProcessedOutboxMessagesService>();

        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(ProcessedOutboxMessagesJob));

            config.AddJob<ProcessedOutboxMessagesJob>(jobKey)
            .AddTrigger(trigger =>
            {
                trigger.ForJob(jobKey)
                .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(1)
                .RepeatForever());
            });
        });

        services.AddQuartzHostedService(option => { option.WaitForJobsToComplete = true; });

        return services;
    }
}
