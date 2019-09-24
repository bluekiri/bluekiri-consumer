using System;
using System.Threading;

namespace Bluekiri.Consumer
{
    public interface IBrokerConsumer : IDisposable
    {
        MessageInfo Consume(CancellationToken stopingToken);        
        bool IsEnabledAutoCommit { get; }
    }
}
