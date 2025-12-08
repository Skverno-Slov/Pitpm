namespace OrderProcessing.Interfaces
{
    /// <summary>
    /// Интерфейс шины сообщений, используемой для уведомлений других сервисов.
    /// </summary>
    public interface IMessageBus
    {
        Task PublishAsync(string topic, object payload);
    }
}
