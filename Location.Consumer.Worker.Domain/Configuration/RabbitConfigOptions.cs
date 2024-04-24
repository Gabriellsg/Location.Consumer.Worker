namespace Location.Consumer.Worker.Domain.Configuration;

public sealed class RabbitConfigOptions
{
    public const string Config = "RabbitConfig";
    public required string LocationReceiverQueueSettingsName { get; set; }
    public required string HostName { get; set; }
    public required string Port { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}