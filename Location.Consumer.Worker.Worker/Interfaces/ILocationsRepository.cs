using Location.Consumer.Worker.Domain.Dtos;

namespace Location.Consumer.Worker.Worker.Interfaces;

public interface ILocationsRepository
{
    Task RegisterLocation(RegisterLocationsModel model);
}