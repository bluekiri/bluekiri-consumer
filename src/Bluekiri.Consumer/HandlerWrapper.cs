using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
    internal abstract class HandlerWrapper
    {
        public abstract Task Handle(IMessage message,CancellationToken cancellationToken, HandlerFactory handlerFactory,
                                    Func<IEnumerable<Func<IMessage, CancellationToken, Task>>, IMessage, CancellationToken, Task> publish);
    }
}