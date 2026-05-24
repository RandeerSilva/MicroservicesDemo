using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messaging.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync<T>(string topic, T message);
    }
}
