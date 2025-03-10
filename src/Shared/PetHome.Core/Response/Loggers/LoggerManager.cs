using Elastic.CommonSchema;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace PetHome.Core.Response.Loggers;

public class LoggerManager
{
    public static Serilog.ILogger InitConfiguration(IConfiguration configuration)
    {
        string indexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower()
            .Replace(".", "-")}-{DateTime.UtcNow:dd-mm-yyyy}";

        return new LoggerConfiguration()
             .WriteTo.Console()
             .WriteTo.Debug()
             .WriteTo.Seq(configuration.GetConnectionString("Seq")!)
             .WriteTo.Elasticsearch([new Uri("http://localhost:9200")], options =>
             {
                 options.DataStream = new DataStreamName(indexFormat);
                 options.TextFormatting = new EcsTextFormatterConfiguration();
                 options.BootstrapMethod = BootstrapMethod.Silent;
             })
             .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
             .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
             .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
             .CreateLogger();
    }
}
