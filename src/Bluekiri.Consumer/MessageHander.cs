using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
    public abstract class MessageHandler
    {
        public abstract Task HandleAsync<T>(T message);

    }
}
