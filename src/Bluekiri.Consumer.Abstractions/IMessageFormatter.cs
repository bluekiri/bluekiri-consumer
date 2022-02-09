using System;

namespace Bluekiri.Consumer.Abstractions
{
    public interface IMessageFormatter
    {
        object Deserialize(byte[] message, Type type);
        string ContentType { get; }
    }
}