using Microsoft.Extensions.Options;
using System;

namespace Bluekiri.Consumer
{
    public class HandlerManager : IHandlerManager
    {
        private readonly HandlerOptions _handlerOptions;

        public HandlerManager(IOptions<HandlerOptions> options)
        {
            _handlerOptions = options.Value;
        }

        public Type GetModelType(string messageType)
        {
            return _handlerOptions.GetModel(messageType);
        }

        public Type GetMessageHandlerType(string messageType)
        {
            return _handlerOptions.GetHandler(messageType);
        }

      
    }

}
