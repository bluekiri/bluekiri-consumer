using System;

namespace Sample.Application
{
    public class Test
    {
        public string To { get; set; }
        public int Val { get; set; }
        public MessageSend Message { get; set; }

    }

    public class MessageSend
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
