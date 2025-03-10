using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Infrastructure.TransactionOutbox;
using Quartz;

namespace PetHome.Accounts.Infrastructure.Inject.TransactionOutbox;
public static class TransactionOutboxDependencyInjection
{
    public static IServiceCollection AddOutboxService(this IServiceCollection services)
    {
        services.AddScoped<ProcessedOutboxMessagesService>();

        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(Infrastructure.TransactionOutbox.ProcessedOutboxMessagesJob));

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
