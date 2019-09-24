using System.Threading;
using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
    public interface IHandlerMessageFactory
    {
        Task Publish(object message, CancellationToken cancellationToken = default);
    }
}