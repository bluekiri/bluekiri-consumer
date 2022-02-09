using Microsoft.Extensions.Options;
using System;

namespace Bluekiri.Consumer
{
    /// <summary>
    /// Handler Manager
    /// </summary>
    public class HandlerManager : IHandlerManager
    {
        private readonly HandlerOptions _handlerOptions;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"><see cref="IOptions{HandlerOptions}"/></param>
        public HandlerManager(IOptions<HandlerOptions> options)
        {
            _handlerOptions = options.Value;
        }
        /// <summary>
        /// Get type of model configured in handleroptions.
        /// </summary>
        /// <param name="messageType"><see cref="string"/></param>
        /// <returns></returns>
        public Type GetModelType(string messageType) => _handlerOptions.GetModel(messageType);
    }
}
