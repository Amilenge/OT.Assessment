namespace OT.Assessment.Core.Messaging
{
    public interface IRabbitMqSettings
    {
        string HostName { get; set; }
        int Port { get; set; }
    }
}
