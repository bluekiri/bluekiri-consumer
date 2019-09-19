using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bluekiri.Consumer
{
    public class ConsumerService<TConsumerOptions> : BackgroundService where TConsumerOptions : ConsumerOptions, new()
    {
        private readonly IBrokerConsumer _consumer;
        private readonly IHandlerManager _handlerManager;
        private readonly IEnumerable<IMessageFormatter> _formatters;
        private readonly ILogger _logger;

        public ConsumerService(
            IBrokerConsumer consumer,
            IHandlerManager handlerManager,
            IEnumerable<IMessageFormatter> formatters,
            ILogger<ConsumerService<TConsumerOptions>> logger)
        {
            _consumer = consumer;
            _handlerManager = handlerManager;
            _formatters = formatters;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    if (consumeResult is null) continue;


                    var contentType = Encoding.UTF8.GetString(consumeResult.Headers["ContentType"]);
                    var messageType = Encoding.UTF8.GetString(consumeResult.Headers["MessageType"]);

                    if (string.IsNullOrWhiteSpace(contentType))
                    {
                        _logger.LogWarning("There is not a valid header with key ContentType");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(messageType))
                    {
                        _logger.LogWarning("There is not a valid header with key MessageType");
                        continue;
                    }

                    var formatter = _formatters.FirstOrDefault(f => f.ContentType.Equals(contentType));

                    if (formatter is null)
                    {
                        _logger.LogError($"There is not a valid formatter provider for formatters of type {contentType}");
                        continue;
                    }
                    var modelType = _handlerManager.GetModelType(messageType);
                    if (modelType is null)
                    {
                        _logger.LogError($"There is not a valid model for messages of type {messageType}");
                        continue;
                    }
                    var handlerType = _handlerManager.GetMessageHandlerType(messageType);
                    if (handlerType is null)
                    {
                        _logger.LogError($"There is not a valid handler for handlers of type {messageType}");
                        continue;
                    }

                    var result = formatter.Deserialize(consumeResult.Message, modelType);

                    var handler = (IMessageHandler)GetDefaultValueForType(handlerType);

                    await handler.HandleAsync(result);
                    if (!_consumer.IsEnnabledAutoCommit)
                    {
                        await consumeResult.CommitAsync().ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Something is wrong");
                }
            }
        }

        private object GetDefaultValueForType(Type handlerType)
        {
            if (handlerType == null)
            {
                throw new ArgumentNullException(nameof(handlerType));
            }
            return Activator.CreateInstance(handlerType);
        }

        public override void Dispose()
        {
            _consumer?.Dispose();
            base.Dispose();
        }
    }
}
