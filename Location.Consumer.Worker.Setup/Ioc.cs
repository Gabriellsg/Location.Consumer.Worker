using Location.Consumer.Worker.Setup.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Location.Consumer.Worker.Setup;

public static class Ioc
{
    public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
    {
        DataModule.Register(services, configuration);
        InfraStructureModule.Register(services, configuration);
        return services;
    }
}