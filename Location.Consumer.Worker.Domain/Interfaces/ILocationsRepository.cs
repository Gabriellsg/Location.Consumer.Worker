using Location.Consumer.Worker.Domain.Dtos;

namespace Location.Consumer.Worker.Domain.Interfaces;

public interface ILocationsRepository
{
    Task RegisterLocation(RegisterLocationsModel model);
}