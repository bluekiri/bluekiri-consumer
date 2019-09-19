using System;
using System.Text.Json;

namespace Bluekiri.Consumer
{
    public class JsonMessageFormatter : IMessageFormatter
    {
        public string ContentType => "application/json";

        public object Deserialize(byte[] message, Type type)
        {
            if (message.Length == 0 || message is null) throw new ArgumentException(nameof(message));
            return JsonSerializer.Deserialize(message,type);
        }
    }
}
