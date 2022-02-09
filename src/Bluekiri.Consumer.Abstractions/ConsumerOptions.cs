using System;
using System.Collections.Generic;

namespace Bluekiri.Consumer.Abstractions
{
    /// <summary>
    /// Consumer options configuration.
    /// </summary>
    public class ConsumerOptions
    {
        private readonly IList<Type> _messageFormatters;
        /// <summary>
        /// Readonly list of added formatters
        /// </summary>
        public IReadOnlyList<Type> MessageFormatters => (IReadOnlyList<Type>)_messageFormatters;

        /// <summary>
        /// Constructor.
        /// Adds Default JsonMessageFormatter
        /// </summary>
        public ConsumerOptions()
        {
            _messageFormatters = new List<Type>();
           
        }
        /// <summary>
        /// Adds new formatter to list.
        /// </summary>
        /// <typeparam name="T"><see cref="IMessageFormatter"/></typeparam>
        public void AddMessageFormatter<T>() where T : IMessageFormatter
        {
            _messageFormatters.Add(typeof(T));
        }
    }
}