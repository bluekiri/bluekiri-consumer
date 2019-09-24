using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
    internal class HandlerWrapperImplementation<TMessage> : HandlerWrapper
        where TMessage : IMessage
    {
        public override Task Handle(IMessage message, CancellationToken cancellationToken, HandlerFactory handlerFactory, Func<IEnumerable<Func<IMessage, CancellationToken, Task>>, IMessage, CancellationToken, Task> publish)
        {
            var handlers = handlerFactory
               .GetInstances<IMessageHandler<TMessage>>()
               .Select(x => new Func<IMessage, CancellationToken, Task>((theNotification, theToken) => x.Handle((TMessage)theNotification, theToken)));

            return publish(handlers, message, cancellationToken);
        }
    }
}