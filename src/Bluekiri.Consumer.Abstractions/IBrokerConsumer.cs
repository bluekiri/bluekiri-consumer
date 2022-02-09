using System;
using System.Threading;

namespace Bluekiri.Consumer.Abstractions
{
    /// <summary>
    /// Abstraction of consumer.
    /// </summary>
    public interface IBrokerConsumer : IDisposable
    {
        /// <summary>
        /// Consume a message from a broker.
        /// </summary>
        /// <param name="stopingToken"><see cref="CancellationToken"/></param>
        /// <returns>A <see cref="MessageInfo"/></returns>
        MessageInfo Consume(CancellationToken stopingToken);
        /// <summary>
        /// Return true if your implementation allows auto commit.
        /// </summary>
        bool IsEnabledAutoCommit { get; }
    }
}