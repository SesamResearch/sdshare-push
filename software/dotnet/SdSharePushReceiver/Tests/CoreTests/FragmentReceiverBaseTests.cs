using System;
using System.Collections.Generic;
using System.Text;
using CoreTests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class FragmentReceiverBaseTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Receive_WithEmptyResources_ArgumentNullException()
        {
            // Arrange
            var receiver = new StubFragmentReceiver();

            // Act
            receiver.Receive(new List<string>(), null, null);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void Receive_MultipleResourcesNotSupported_ThrowsInvalidOperationException()
        {
            // Arrange
            var receiver = new StubFragmentReceiver{Batching = false};

            // Act
            receiver.Receive(new List<string>{"http://resource1", "http://resource2"}, null, null);
        }

        [TestMethod]
        public void Receive_MultipleResourcesEmptyPayload_DeletesResources()
        {
            // Arrange
            var receiver = new StubFragmentReceiver { Batching = true };

            // Act
            receiver.Receive(new List<string> { "http://resource1", "http://resource2" }, null, null);

            // Assert
            Assert.AreEqual(2, receiver.DeletedResources.Count);
            Assert.IsTrue(receiver.DeletedResources.Contains("http://resource1"));
            Assert.IsTrue(receiver.DeletedResources.Contains("http://resource2"));
        }

        [TestMethod]
        public void Receive_MultipleResourcesEmptyPayload_ReceiveCoreNotSents()
        {
            // Arrange
            var receiver = new StubFragmentReceiver { Batching = true };

            // Act
            receiver.Receive(new List<string> { "http://resource1", "http://resource2" }, null, null);

            // Assert
            Assert.IsFalse(receiver.ReceiveCoreReceived);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void Receive_MultipleResourcesMissingTriples_ThrowsInvalidOperationException()
        {
            // Arrange
            var receiver = new StubFragmentReceiver { Batching = true };
            var sb = new StringBuilder();
            sb.AppendLine("<http://resource1> <http://somePredicate> 123");

            // Act
            receiver.Receive(new List<string> { "http://resource1", "http://resource2" }, null, sb.ToString());
        }

        [TestMethod]
        public void Receive_MultipleResourcesValidPayload_PayloadReceived()
        {
            // Arrange
            var receiver = new StubFragmentReceiver { Batching = true };
            var sb = new StringBuilder();
            sb.AppendLine("<http://resource1> <http://somePredicate> 123");
            sb.AppendLine("<http://resource2> <http://somePredicate> 456");

            // Act
            receiver.Receive(new List<string> { "http://resource1", "http://resource2" }, null, sb.ToString());

            // Assert
            Assert.IsTrue(receiver.ReceiveCoreReceived);
        }
    }
}
