using Location.Consumer.Worker.Domain.Configuration;
using Location.Consumer.Worker.Domain.Dtos;
using Location.Consumer.Worker.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Location.Consumer.Worker.Worker;

public sealed class QueueConsumer : BackgroundService
{
    private readonly ILogger<QueueConsumer> _logger;
    private readonly IOptionsMonitor<RabbitConfigOptions> _rabbitConfigOptions;
    private readonly ILocationsRepository _locationsRepository;

    public QueueConsumer(
        ILogger<QueueConsumer> logger,
        IOptionsMonitor<RabbitConfigOptions> rabbitConfigOptions,
        ILocationsRepository locationsRepository)
    {
        _logger = logger;
        _rabbitConfigOptions = rabbitConfigOptions;
        _locationsRepository = locationsRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Woker Started at: {0}", DateTimeOffset.Now);

        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "location-receiver-queue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        _logger.LogInformation("[*] Waiting for messages.");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                channel.BasicAck(eventArgs.DeliveryTag, false);

                var deserializedMessage = JsonSerializer.Deserialize<RegisterLocationsModel>(message);

                await _locationsRepository.RegisterLocation(deserializedMessage!);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                channel.BasicNack(eventArgs.DeliveryTag, false, true);
            }
        };

        channel.BasicConsume(queue: "location-receiver-queue",
                             autoAck: false,
                             consumer: consumer);

        _logger.LogInformation("Woker Finished at: {0}", DateTimeOffset.Now);
    }
}