﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bluekiri.Consumer.Tests
{
    [TestClass]
    public class HandlerOptionsTest
    {

        [TestMethod]
        public void GetModel_ExpectedModel_Model_Exists()
        {
            // Arrange
            var options = new HandlerOptions();
            
            // Act
            var modelType = options.GetModel("test");

            // Assert
            Assert.IsInstanceOfType(modelType, typeof(MessageTest));
        }

    }
}
