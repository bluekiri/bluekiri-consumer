using Bluekiri.Consumer;
using System;
using System.Threading.Tasks;

namespace Sample.Application
{
    [MessageType("sample-message", typeof(Test))]
    public class SampleHandler : IMessageHandler
    {
        public  Task HandleAsync(object message)
        {
            var testMessage = (Test)message;
            return Task.CompletedTask;
        }
    }

    [MessageType("sample-messagesend", typeof(MessageSend))]
    public class SampleHandlerSend : IMessageHandler
    {
        public  Task HandleAsync(object message)
        {
            var testMessage = (MessageSend)message;
            return Task.CompletedTask;
        }
    }
}
