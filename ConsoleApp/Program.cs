

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Somfic.Logging.Console;

namespace ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((context, service) =>
                {
                    service.AddTransient<Core>();
                })
                .ConfigureLogging((context, logger) =>
                {
                    logger.ClearProviders();
                    logger.AddProvider(new ConsoleLoggerProvider());
                    logger.SetMinimumLevel(LogLevel.Trace);
                })
                .Build();

            ActivatorUtilities.CreateInstance<Core>(host.Services).Run().GetAwaiter().GetResult();



            Thread.Sleep(-1);
        }
    }

    public class Core
    {
        private readonly ILogger<Core> _log;
        private readonly IConfiguration _config;

        public Core(ILogger<Core> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public async Task Run()
        {

            int i = 1;

            _log.LogTrace(new Exception("This is an exception message"), "This is an example logging string. Anything else could be added here.");
            _log.LogDebug(new Exception("This is an exception message"), "This is an example logging string. Anything else could be added here.");
            _log.LogInformation(new Exception("This is an exception message"), "This is an example logging string. Anything else could be added here.");
            _log.LogWarning(new Exception("This is an exception message"), "This is an example logging string. Anything else could be added here.");
            _log.LogError(new Exception("This is an exception message"), "This is an example logging string. Anything else could be added here.");
            _log.LogCritical(new Exception("This is an exception message"), "This is an example logging string. Anything else could be added here.");

            Thread.Sleep(-1);
        }
    }
}
