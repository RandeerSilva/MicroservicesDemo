using DotNetCore.CAP;
using Shared.Messaging.Abstractions;


namespace Shared.Messaging.Cap
{
    public sealed class CapEventBus(ICapPublisher cap) : IEventBus
    {
        public Task PublishAsync<T>(string topic, T message)
            => cap.PublishAsync(topic, message);
    }
}
