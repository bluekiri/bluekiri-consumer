using System;
using System.Collections.Generic;

namespace Bluekiri.Consumer
{
    public class ConsumerOptions
    {
        internal IList<Type> MessageFormatters { get; }
        
        public ConsumerOptions()
        {
            MessageFormatters = new List<Type>
            {
                typeof(JsonMessageFormatter)
            };
        }
        
        public void AddMessageFormatter<T>() where T : IMessageFormatter
        {
            MessageFormatters.Add(typeof(T));
        }
    }
}
