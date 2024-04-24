namespace Location.Consumer.Worker.Domain.Dtos;

public sealed record RegisterLocationsModel(
    int DeliverymanId,
    int LocationPlansId,
    int MotorcycleId,
    DateTime LocationStart,
    DateTime LocationExpectedEnd);