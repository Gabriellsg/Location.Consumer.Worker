using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Location.Consumer.Worker.Worker.Helpers;

public static class DefaultStartup
{
    public static IHostBuilder Initialize(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(new Action<HostBuilderContext, IConfigurationBuilder>(BuilderConfiguration));
    }

    public static void BuilderConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
    {
        builder.AddEnvironment(context.HostingEnvironment.ContentRootPath).AddNamedEnvironment(builder).AddEnvironmentVariables();
    }

    private static IConfigurationBuilder AddNamedEnvironment(this IConfiguration configuration, IConfigurationBuilder configBuilder)
    {
        string? value = configuration.GetSection("Environment").Value ?? null;
        return configBuilder.AddJsonFile("appsettings." + (string.IsNullOrEmpty(value) ? "Development" : value) + ".json", optional: true, reloadOnChange: true);
    }

    private static IConfigurationRoot AddEnvironment(this IConfigurationBuilder configBuilder, string directory)
    {
        return configBuilder.SetBasePath(directory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
    }
}