using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
    public abstract class MessageInfo 
    {
        public virtual string Key { get; set; }
        public virtual IDictionary<string, byte[]> Headers { get; set; }
        public virtual byte[] Message { get; set; }

        protected MessageInfo()
        {
            Headers = new Dictionary<string, byte[]>();            
        }

        public abstract Task CommitAsync();
    }
}
