using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace PetHome.Core.Response.Loggers;

public class SeqLogger
{
    public static Serilog.ILogger InitDefaultSeqConfiguration(IConfiguration configuration)
    {
        return new LoggerConfiguration()
             .WriteTo.Console()
             .WriteTo.Debug()
             .WriteTo.Seq(configuration.GetConnectionString("Seq")!)
             .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
             .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
             .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
             .CreateLogger();
    }
}
