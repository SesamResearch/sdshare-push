using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SdShare;
using SdShare.Idempotency;

namespace CoreTests.Idempotency
{
    [TestClass]
    public class IdempotentFragmentReceiverWrapperTests
    {
        private Mock<IFragmentReceiver> _receiverMock;

        [TestInitialize]
        public void SetUp()
        {
            _receiverMock = new Mock<IFragmentReceiver>();
        }

        [TestMethod]
        public void Constructor_ValidInput_ConstructsInstance()
        {
            // Arrange
            var expiration = TimeSpan.FromSeconds(2);
            const CacheMethod cacheMethod = CacheMethod.Memory;

            // Act
            var wrapper = new IdempotentFragmentReceiverWrapper(_receiverMock.Object, expiration, cacheMethod);

            // Assert
            Assert.IsNotNull(wrapper);
        }

        [TestMethod]
        public void Receive_ValidInput_IsSentThrough()
        {
            // Arrange
            var expiration = TimeSpan.FromSeconds(2);
            const CacheMethod cacheMethod = CacheMethod.Memory;
            var wrapper = new IdempotentFragmentReceiverWrapper(_receiverMock.Object, expiration, cacheMethod);
            const string resourceUri = "http://resourceUri";
            const string graphUri = "http://graphUri";

            var payload = Resources.Triples.Replace("Kristin", GetRandomString());

            // Act
            wrapper.Receive(resourceUri, graphUri, payload);

            // Assert
            _receiverMock.Verify(r => r.Receive(resourceUri, graphUri, payload), Times.Once);
        }

        [TestMethod]
        public void Receive_Twice_IsSentThroughOnlyOnce()
        {
            // Arrange
            var expiration = TimeSpan.FromSeconds(2);
            const CacheMethod cacheMethod = CacheMethod.Memory;
            var wrapper = new IdempotentFragmentReceiverWrapper(_receiverMock.Object, expiration, cacheMethod);
            const string resourceUri = "http://resourceUri";
            const string graphUri = "http://graphUri";

            var payload = Resources.Triples.Replace("Kristin", GetRandomString());

            // Act
            wrapper.Receive(resourceUri, graphUri, payload);
            wrapper.Receive(resourceUri, graphUri, payload);

            // Assert
            _receiverMock.Verify(r => r.Receive(resourceUri, graphUri, payload), Times.Once);
        }

        [TestMethod]
        public void Receive_TwiceWithTimeout_IsSentThroughTwice()
        {
            // Arrange
            var expiration = TimeSpan.FromSeconds(2);
            const CacheMethod cacheMethod = CacheMethod.Memory;
            var wrapper = new IdempotentFragmentReceiverWrapper(_receiverMock.Object, expiration, cacheMethod);
            const string resourceUri = "http://resourceUri";
            const string graphUri = "http://graphUri";

            var payload = Resources.Triples.Replace("Kristin", GetRandomString());

            // Act
            wrapper.Receive(resourceUri, graphUri, payload);
            Thread.Sleep(2100);
            wrapper.Receive(resourceUri, graphUri, payload);

            // Assert
            _receiverMock.Verify(r => r.Receive(resourceUri, graphUri, payload), Times.Exactly(2));
        }

        private string GetRandomString()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
