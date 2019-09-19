using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Threading.Tasks;

namespace Bluekiri.Consumer.Tests
{
    [TestClass]
    public class JsonMessageFormatterTest
    {

        [TestMethod]
        public void Deserialize_With_CorrectType()
        {
            // Arrange
            var jsonMessageFormatter = new JsonMessageFormatter();
            var type = typeof(MessageTest);
            var message = Encoding.UTF8.GetBytes("{ \"MyProperty\": \"Hello world\" }");
            // Act
            var objectUnderTest = jsonMessageFormatter.Deserialize(message, type);

            // Assert
            Assert.IsInstanceOfType(objectUnderTest, type);
        }

        [TestMethod]
        public void Deserialize_ClassUnderTest_Correctly_Deserialized()
        {
            // Arrange
            var jsonMessageFormatter = new JsonMessageFormatter();
            var type = typeof(MessageTest);
            var message = Encoding.UTF8.GetBytes("{ \"MyProperty\": \"Hello world\" }");
            // Act
            var objectUnderTest = jsonMessageFormatter.Deserialize(message, type);

            // Assert
            Assert.IsNotNull(objectUnderTest);
            var result = (MessageTest)objectUnderTest;
            Assert.AreEqual("Hello world", result.MyProperty);

        }
       
    }
    public class MessageTest
    {
        public string MyProperty { get; set; }
    }
    public class HandlerTest : MessageHandler
    {
        public override Task HandleAsync<MessageTest>(MessageTest message)
        {
            return Task.CompletedTask;
        }
    }
}
