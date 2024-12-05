using Serilog;
using Serilog.Events;

namespace PetHome.API.Loggers;

public class SeqLogger
{
    public static Serilog.ILogger InitDefaultSeqConfiguration()
    {
        return new LoggerConfiguration()
             .WriteTo.Console()
             .WriteTo.Debug()
             .WriteTo.Seq("http://localhost:5341")
             .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
             .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
             .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
             .CreateLogger();
    }
}
