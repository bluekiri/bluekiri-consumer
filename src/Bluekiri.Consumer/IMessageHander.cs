using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
     public interface IMessageHandler
    {
        Task HandleAsync(object message);
    }
  
}
