using System;
using System.Threading;

namespace Bluekiri.Consumer
{
    /// <summary>
    /// Abstraction of consumer.
    /// </summary>
    public interface IBrokerConsumer : IDisposable
    {
        MessageInfo Consume(CancellationToken stopingToken);        
        bool IsEnabledAutoCommit { get; }
    }
}
