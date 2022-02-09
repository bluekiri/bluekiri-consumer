using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bluekiri.Consumer.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bluekiri.Consumer.Tests
{
    [TestClass]
    public class ConsumerServiceTest
    {
        [TestMethod]
        public async Task ConsumeAsync_Ends_Correctly()
        {

            var mockBrokerConsumer = new Mock<IBrokerConsumer>();
            mockBrokerConsumer.Setup(s => s.Consume(It.IsAny<CancellationToken>())).Returns(new ConsumeResponseFake {
                 Headers = new Dictionary<string, byte[]>
                 {
                     { "ContentType", Encoding.UTF8.GetBytes("application/json") },
                     { "MessageType", Encoding.UTF8.GetBytes("modelexpected") }
                 }
            });

            var mockHandlerManager = new Mock<IHandlerManager>();
            mockHandlerManager.Setup(s => s.GetModelType("modelexpected")).Returns(typeof(ModelExpected));

            var mockFormatter = new Mock<IMessageFormatter>();
            mockFormatter.Setup(s => s.ContentType).Returns("application/json");
            mockFormatter.Setup(s => s.Deserialize(It.IsAny<byte[]>(), It.IsAny<Type>())).Returns(new ModelExpected { TestMessage = "test" });

            var mockFactory = new Mock<IHandlerMessageFactory>();
            mockFactory.Setup(s => s.ExecuteAsync(It.IsAny<object>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var mockLogger = new NullLogger<ConsumerService<FakeConsumeOptions>>();



            var consume = new FakeBackgroundService(mockBrokerConsumer.Object,
                mockHandlerManager.Object,
                mockFactory.Object,
                new List<IMessageFormatter> { mockFormatter.Object },
                mockLogger);

            var cts = new CancellationTokenSource(1000);
            CancellationToken token = cts.Token;
            await consume.ExposedExecuteAsync(token);
        }

        private class ConsumeResponseFake : MessageInfo
        {

            public override Task CommitAsync()
            {
                return Task.CompletedTask;
            }
        }
        private class ModelExpected : IMessage
        {
            public string TestMessage { get; set; }
        }



    }
    public class FakeConsumeOptions : ConsumerOptions
    {

    }
    public class FakeBackgroundService : ConsumerService<FakeConsumeOptions>
    {
        public FakeBackgroundService(IBrokerConsumer consumer,
        IHandlerManager handlerManager,
        IHandlerMessageFactory factory,
        IEnumerable<IMessageFormatter> formatters,
        ILogger<ConsumerService<FakeConsumeOptions>> logger) : base(consumer, handlerManager, factory, formatters, logger)
        {

        }

        public Task ExposedExecuteAsync(CancellationToken cancellationToken)
        {
            return base.ExecuteAsync(cancellationToken);
        }
    }

}
