using System;

namespace Bluekiri.Consumer
{
    public interface IMessageFormatter
    {
        object Deserialize(byte[] message, Type type);
        string ContentType { get; }
    }

}
