using System.Collections.Generic;

namespace Bluekiri.Consumer.Kafka
{
    public class KafkaConsumerOptions : ConsumerOptions
    {
        public IList<string> Topics { get;  }
        public IList<KeyValuePair<string, string>> KafkaConfig { get; }

        public KafkaConsumerOptions()
        {
            Topics = new List<string>();
            KafkaConfig = new List<KeyValuePair<string, string>>();
        }
        public void SetProperty(string key, string value)
        {
            KafkaConfig.Add(new KeyValuePair<string, string>(key, value));
        }
    }
}
