using System.Threading;
using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
    public interface IHandlerMessageFactory
    {
        Task ExecuteAsync(object message, CancellationToken cancellationToken = default);
    }
}