namespace OT.Assessment.Core.Messaging
{
    public class RabbitMqSettings : IRabbitMqSettings
    {
        public string HostName { get; set; }
        public int Port { get; set; }

        public RabbitMqSettings(string hostName, int port = 5672)
        {
            HostName = hostName;
            Port = port;
        }
    }
}
