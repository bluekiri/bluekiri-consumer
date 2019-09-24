using System;
using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
    public interface IHandlerManager
    {
        Type GetModelType(string messageType);
    }
}