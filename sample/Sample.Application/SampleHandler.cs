using Bluekiri.Consumer;
using System;
using System.Threading.Tasks;

namespace Sample.Application
{
    [MessageType("sample-message", typeof(Test))]
    public class SampleHandler : MessageHandler
    {
        public override Task HandleAsync<Test>(Test message)
        {
            return Task.CompletedTask;
        }
    }

    [MessageType("sample-messagesend", typeof(MessageSend))]
    public class SampleHandlerSend : MessageHandler
    {
        public override Task HandleAsync<MessageSend>(MessageSend message)
        {

            return Task.CompletedTask;
        }
    }



}
