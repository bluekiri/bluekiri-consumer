using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;

namespace Bluekiri.Consumer.Kafka
{
    public class KafkaConsumer : IBrokerConsumer
    {
        private readonly IConsumer<string, byte[]> _consumer;

        private readonly IList<string> _topics;
        private readonly IList<KeyValuePair<string, string>> _properties;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly ConsumerConfig _consumerConfig;

        public bool IsEnnabledAutoCommit { get; }

        public KafkaConsumer(IOptions<KafkaConsumerOptions> options, ILogger<KafkaConsumer> logger)
        {
            _topics = options.Value.Topics;
            _properties = options.Value.KafkaConfig;
            _logger = logger;

            _consumerConfig = new ConsumerConfig(_properties)
            {
                EnablePartitionEof = true
            };

            IsEnnabledAutoCommit = _consumerConfig.EnableAutoCommit ?? true;

            _consumer = new ConsumerBuilder<string, byte[]>(_consumerConfig)
                .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}"))
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    _logger.LogInformation($"Assigned partitions: [{string.Join(", ", partitions)}]");
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    _logger.LogWarning($"Revoking assignment: [{string.Join(", ", partitions)}]");
                })
                .Build();

            _consumer.Subscribe(_topics);
        }

        public MessageInfo Consume(CancellationToken cancellationToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                if (consumeResult.IsPartitionEOF)
                {
                    _logger.LogInformation($"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");
                    return null;
                }
                else
                {
                    var headers = consumeResult.Headers;
                    var messageInfo = new MessageInfo
                    {
                        Key = consumeResult.Key,
                        Message = consumeResult.Message.Value
                    };
                    messageInfo.SetResult(consumeResult);
                    foreach (var h in headers)
                    {
                        messageInfo.Headers.Add(h.Key, h.GetValueBytes());
                    }

                    return messageInfo;
                }
            }
            catch (ConsumeException e)
            {
                _logger.LogError(e, "Consume error:{0}", e.Error.Reason);
                throw;
            }
        }

        public void Dispose()
        {
            _consumer?.Dispose();
        }

        public void CommitMessage(MessageInfo cr)
        {
            try
            {
                _consumer.Commit(cr.GetResult<ConsumeResult<string,byte[]>>());
            }

            catch (KafkaException e)
            {
                _logger.LogError(e, $"ERROR: Kafka error: {e.Error.Reason}");
            }
        }
    }
    
}
