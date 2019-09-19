using System;
using System.Collections.Generic;

namespace Bluekiri.Consumer
{
    public class MessageInfo 
    {
        private object Result { get;  set; }
        public void SetResult<T>(T result)
        {
            Result = result;
        }
        public T GetResult<T>()
        {
            return (T)Result;
        }
        public virtual string Key { get; set; }
        public virtual IDictionary<string, byte[]> Headers { get; set; }
        public virtual byte[] Message { get; set; }

        public MessageInfo()
        {
            Headers = new Dictionary<string, byte[]>();
        }

       
    }

   

}
