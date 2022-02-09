using System.Threading;
using System.Threading.Tasks;

namespace Bluekiri.Consumer.Abstractions
{
    /// <summary>
    /// Handler interface.
    /// </summary>
    /// <typeparam name="TMessage"><see cref="IMessage"/></typeparam>
    public interface IMessageHandler<in TMessage>
    where TMessage : IMessage
    {
        /// <summary>
        /// Handle Method.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task Handle(TMessage message, CancellationToken cancellationToken = default);
    }
}