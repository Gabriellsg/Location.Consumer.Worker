using Location.Consumer.Worker.Worker;
using Location.Consumer.Worker.Worker.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using Location.Consumer.Worker.Setup;
using Microsoft.Extensions.Configuration;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private static void Main(string[] args)
    {

        DefaultStartup
            .Initialize(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                      .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.ConfigureContainer(context.Configuration);
                
                services.AddHostedService<QueueConsumer>();

            })
            .Build()
            .RunAsync();
    }
}