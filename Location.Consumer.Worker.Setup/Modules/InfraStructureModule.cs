using Location.Consumer.Worker.Domain.Interfaces;
using Location.Consumer.Worker.Infra.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Location.Consumer.Worker.Setup.Modules;
public static class InfraStructureModule
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILocationsRepository, LocationsRepository>();
    }
}