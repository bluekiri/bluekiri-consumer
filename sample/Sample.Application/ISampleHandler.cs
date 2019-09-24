using System.Threading.Tasks;

namespace Sample.Application
{
    public interface ISampleHandler
    {
        Task HandleAsync(TestClass message);
    }
}