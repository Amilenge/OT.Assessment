namespace OT.Assessment.Core.Messaging.Senders
{
    public interface IMessageSender<T>
    {
        Task SendAsync(T message);
    }
}
