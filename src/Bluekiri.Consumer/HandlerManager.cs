using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
    public class HandlerManager : IHandlerManager
    {
        private readonly HandlerOptions _handlerOptions;

        public HandlerManager(IOptions<HandlerOptions> options)
        {
            _handlerOptions = options.Value;
        }
        public Type GetModelType(string messageType) => _handlerOptions.GetModel(messageType);
    }
}
