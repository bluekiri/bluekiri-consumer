using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bluekiri.Consumer
{
    /// <summary>
    /// Worker for consume.
    /// </summary>
    /// <typeparam name="TConsumerOptions"><see cref="ConsumerOptions"/></typeparam>
    public class ConsumerService<TConsumerOptions> : BackgroundService where TConsumerOptions : ConsumerOptions, new()
    {
        private readonly IBrokerConsumer _consumer;
        private readonly IHandlerManager _handlerManager;
        private readonly IHandlerMessageFactory _factory;
        private readonly IEnumerable<IMessageFormatter> _formatters;
        private readonly ILogger _logger;

        private const string ContentType = "ContentType";
        private const string MessageType = "MessageType";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="consumer"><see cref="IBrokerConsumer"/></param>
        /// <param name="handlerManager"><see cref="IHandlerManager"/></param>
        /// <param name="factory"><see cref="IHandlerMessageFactory"/></param>
        /// <param name="formatters"><see cref="IEnumerable{IMessageFormatter}"/></param>
        /// <param name="logger"><see cref="ILogger{ConsumerService}"/></param>
        public ConsumerService(
            IBrokerConsumer consumer,
            IHandlerManager handlerManager,
            IHandlerMessageFactory factory,
            IEnumerable<IMessageFormatter> formatters,
            ILogger<ConsumerService<TConsumerOptions>> logger)
        {
            _consumer = consumer;
            _handlerManager = handlerManager;
            _factory = factory;
            _formatters = formatters;
            _logger = logger;
        }
        /// <summary>
        /// Backgroundservice override.
        /// </summary>
        /// <param name="stoppingToken"><see cref="CancellationToken"/></param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    if (consumeResult is null) continue;
                    if (!consumeResult.Headers.ContainsKey(ContentType))
                    {
                        _logger.LogError("Headers not contains key ContentType");
                        continue;
                    }
                    if (!consumeResult.Headers.ContainsKey(MessageType))
                    {
                        _logger.LogError("Headers not contains key MessageType");
                        continue;
                    }

                    var contentType = Encoding.UTF8.GetString(consumeResult.Headers[ContentType]);
                    var messageType = Encoding.UTF8.GetString(consumeResult.Headers[MessageType]);

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

                    var formatter = _formatters.Where(f => f.ContentType.Equals(contentType)).FirstOrDefault();

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
                    var result = formatter.Deserialize(consumeResult.Message, modelType);
                    await _factory.ExecuteAsync(result, stoppingToken).ConfigureAwait(false);


                    if (!_consumer.IsEnabledAutoCommit)
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

        /// <summary>
        /// base dispose
        /// </summary>
        public override void Dispose()
        {
            _consumer?.Dispose();
            base.Dispose();
        }
    }


}


