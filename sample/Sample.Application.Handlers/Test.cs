using Bluekiri.Consumer;
using Bluekiri.Consumer.Abstractions;
using System;

namespace Sample.Application
{
    public class TestClass : IMessage
    {
        public string To { get; set; }
        public int Val { get; set; }


    }

    public class MessageSend: IMessage
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
