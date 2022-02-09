using System.Collections.Generic;

namespace Bluekiri.Consumer.Kafka
{
    /// <summary>
    /// Kafka consumer configuration.
    /// </summary>
    public class KafkaConsumerOptions : ConsumerOptions
    {
        internal IList<string> Topics;
        internal IList<KeyValuePair<string, string>> KafkaConfig;
        /// <summary>
        /// Constructor.
        /// </summary>
        public KafkaConsumerOptions()
        {
            Topics = new List<string>();
            KafkaConfig = new List<KeyValuePair<string, string>>();
        }
        /// <summary>
        /// Set the configuration for the consumer.
        /// </summary>
        /// <param name="key">Configuration name</param>
        /// <param name="value">Configuration value</param>
        public void SetProperty(string key, string value)
        {
            KafkaConfig.Add(new KeyValuePair<string, string>(key, value));
        }
        /// <summary>
        /// Add a topic into topics consumer list.
        /// </summary>
        /// <param name="topic"></param>
        public void AddTopic(string topic) => Topics.Add(topic);


    }
}
