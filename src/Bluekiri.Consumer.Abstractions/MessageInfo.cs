using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bluekiri.Consumer.Abstractions
{
    /// <summary>
    /// MessageInfo
    /// </summary>
    public abstract class MessageInfo
    {
        public virtual string Key { get; set; }
        /// <summary>
        /// Message Header
        /// </summary>
        public virtual IDictionary<string, byte[]> Headers { get; set; }
        /// <summary>
        /// Message Value
        /// </summary>
        public virtual byte[] Message { get; set; }
 
        /// <summary>
        /// Constructor.
        /// </summary>
        protected MessageInfo()
        {
            Headers = new Dictionary<string, byte[]>();
        }
        /// <summary>
        /// Commit if it is necessary the message.
        /// </summary>
        /// <returns></returns>
        public abstract Task CommitAsync();
    }
}