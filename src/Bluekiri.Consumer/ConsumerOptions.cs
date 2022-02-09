using System;
using System.Collections.Generic;

namespace Bluekiri.Consumer
{
    /// <summary>
    /// Consumer options configuration.
    /// </summary>
    public class ConsumerOptions
    {
        internal IList<Type> MessageFormatters { get; }
      
        
        /// <summary>
        /// Constructor.
        /// Adds Default JsonMessageFormatter
        /// </summary>
        public ConsumerOptions()
        {
            MessageFormatters = new List<Type>
            {
                typeof(JsonMessageFormatter)
            };
        }
        /// <summary>
        /// Adds new formatter to list.
        /// </summary>
        /// <typeparam name="T"><see cref="IMessageFormatter"/></typeparam>
        public void AddMessageFormatter<T>() where T : IMessageFormatter
        {
            MessageFormatters.Add(typeof(T));
        }
    }
}
