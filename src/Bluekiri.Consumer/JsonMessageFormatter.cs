using Bluekiri.Consumer.Abstractions;
using System;
using System.Text.Json;

namespace Bluekiri.Consumer
{
    /// <summary>
    /// Default json formatter
    /// </summary>
    public class JsonMessageFormatter : IMessageFormatter
    {
        /// <summary>
        /// Header content type
        /// </summary>
        public string ContentType => "application/json";

        /// <summary>
        /// returns a object with desired type from json byte[]
        /// </summary>
        /// <param name="message"><see cref="byte"/></param>
        /// <param name="type"><see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="JsonException"></exception>
        public object Deserialize(byte[] message, Type type)
        {
            if (message.Length == 0 || message is null) throw new ArgumentException(nameof(message));
            return JsonSerializer.Deserialize(message,type);
        }
    }
}
