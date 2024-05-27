using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;


namespace LoggingSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", optional: true)
                .Build();
            
            Console.WriteLine("Hello from System.Console!");

            var providers = new LoggerProviderCollection();
            Log.Logger = new LoggerConfiguration()
                //.MinimumLevel.Debug()
                //.WriteTo.Console()
                .WriteTo.Providers(providers)
                .WriteTo.File("logs/logs.txt")
                .CreateLogger();

            using ILoggerFactory factory = LoggerFactory.Create(builder =>
            {
                //builder.SetMinimumLevel(LogLevel.Warning);
                builder.AddConfiguration(config);
                builder.AddConsole();
                builder.AddSerilog();

                // builder.AddFilter(level => true);
            }
            );

            var logger = factory.CreateLogger("Program");

            logger.LogTrace("Hello form {name}", "Trace");
            logger.LogDebug("Hello form {name}", "Debug");
            logger.LogInformation("Hello form {name}", "Information");
            logger.LogWarning("Hello form {name}", "Warning");
            logger.LogError("Hello form {name}", "Error");
            logger.LogCritical("Hello form {name}", "Critical");
        }
    }
}
