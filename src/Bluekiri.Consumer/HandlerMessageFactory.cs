using Bluekiri.Consumer.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bluekiri.Consumer
{
    public class HandlerMessageFactory : IHandlerMessageFactory
    {
        private readonly HandlerFactory _handlerFactory;
        private static readonly ConcurrentDictionary<Type, HandlerWrapper> _messageHandlers = new ConcurrentDictionary<Type, HandlerWrapper>();
        public HandlerMessageFactory(HandlerFactory handlerFactory)
        {
            _handlerFactory = handlerFactory;
        }
        public Task ExecuteAsync(object message, CancellationToken cancellationToken = default)
        {
            if (message is IMessage instance)
            {
                return PublishMessage(instance, cancellationToken);
            }
            throw new ArgumentException($"{nameof(message)} does not implement ${nameof(IMessage)}");
        }

        protected virtual async Task PublishCore(IEnumerable<Func<IMessage, CancellationToken, Task>> allHandlers, IMessage notification, CancellationToken cancellationToken)
        {
            foreach (var handler in allHandlers)
            {
                await handler(notification, cancellationToken).ConfigureAwait(false);
            }
        }

        private Task PublishMessage(IMessage message, CancellationToken cancellationToken = default)
        {
            var type = message.GetType();
            var handler = _messageHandlers.GetOrAdd(type,
                t => (HandlerWrapper)Activator.CreateInstance(typeof(HandlerWrapperImplementation<>)
                .MakeGenericType(type)));

            return handler.Handle(message, cancellationToken, _handlerFactory, PublishCore);
        }
    }
}