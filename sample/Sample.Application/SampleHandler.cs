using Bluekiri.Consumer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application
{
    [MessageType("sample-message", typeof(TestClass))]
    public class SampleHandler : IMessageHandler<TestClass> 
    {
        private readonly ILogger<SampleHandler> _logger;

        public SampleHandler(ILogger<SampleHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(TestClass message, CancellationToken cancellationToken= default)
        {
            _logger.LogError(message.To);
            return Task.CompletedTask;
        }
    }
    [MessageType("sample-messagesend", typeof(MessageSend))]
    public class SampleHandler2 : IMessageHandler<MessageSend>
    {
        private readonly ILogger<SampleHandler2> _logger;

        public SampleHandler2(ILogger<SampleHandler2> logger)
        {
            _logger = logger;
        }
        public Task Handle(MessageSend message, CancellationToken cancellationToken = default)
        {
            _logger.LogError(message.Message);
            return Task.CompletedTask;
        }
    }


}
