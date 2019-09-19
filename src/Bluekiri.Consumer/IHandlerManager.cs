using System;

namespace Bluekiri.Consumer
{
    public interface IHandlerManager
    {
        Type GetMessageHandlerType(string messageType);
        Type GetModelType(string messageType);
    }
}