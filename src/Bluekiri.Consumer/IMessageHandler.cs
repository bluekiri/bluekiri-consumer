using System.Threading;
using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
    public interface IMessageHandler<in TMessage>
        where TMessage : IMessage
    {
        Task Handle(TMessage message, CancellationToken cancellationToken=default);
    }
}