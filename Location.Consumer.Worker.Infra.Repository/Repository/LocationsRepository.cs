using Dapper;
using Location.Consumer.Worker.Domain.Dtos;
using Location.Consumer.Worker.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace Location.Consumer.Worker.Infra.Data.Repository;

public sealed class LocationsRepository : ILocationsRepository
{
    private readonly ILogger<LocationsRepository> _logger;
    private readonly IDbConnection _connection;
    public LocationsRepository(ILogger<LocationsRepository> logger, IDbConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    public async Task RegisterLocation(RegisterLocationsModel model)
    {
        try
        {
            var result = await _connection.ExecuteAsync($@"
                    INSERT INTO LOCATIONS 
                    (DELIVERYMAN_ID, LOCATION_PLANS_ID, MOTORCYCLE_ID, LOCATION_START, LOCATION_EXPECTED_END)
                    VALUES(@DeliverymanId, @LocationPlansId, @MotorcycleId, @LocationStart, @LocationExpectedEnd)",
                    new
                    {
                        model.DeliverymanId,
                        model.LocationPlansId,
                        model.MotorcycleId,
                        model.LocationStart,
                        model.LocationExpectedEnd,
                    });
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError(ex, "Erro ao processar a requisição");
            throw;
        }
    }
}