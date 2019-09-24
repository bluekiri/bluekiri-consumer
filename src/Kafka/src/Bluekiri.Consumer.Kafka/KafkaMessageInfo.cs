using System.Threading.Tasks;
using Confluent.Kafka;

namespace Bluekiri.Consumer.Kafka
{
    class KafkaMessageInfo : MessageInfo
    {
        private readonly KafkaConsumer _consumer;
        internal KafkaMessageInfo(KafkaConsumer consumer) : base()
        {
            _consumer = consumer;
        }

        internal ConsumeResult<string, byte[]> Result { get; set; }

        public override Task CommitAsync()
        {            
            _consumer.Commit(Result);            
            return Task.CompletedTask;
        }

    }


}