using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SdShare.Configuration;
using SdShare.Idempotency;
using TestUtils;

namespace CoreTests.Configuration
{
    [TestClass]
    public class EndpointConfigurationTests
    {
        [TestMethod]
        public void GetReceivers_GraphIsNull_GetsExpectedReceivers()
        {
            // Act
            var receivers = EndpointConfiguration.GetConfiguredReceivers(null);

            // Assert
            Assert.AreEqual(1, receivers.Count());
            Assert.IsTrue(receivers.Any(r => typeof(StubFragmentReceiverTypeA) == r.GetType()));
        }

        [TestMethod]
        public void GetReceivers_WithConfiguredGraph_GetsExpectedReceivers()
        {
            // Act
            var receivers = EndpointConfiguration.GetConfiguredReceivers("http://test/graph");

            // Assert
            Assert.AreEqual(2, receivers.Count());
            Assert.IsTrue(receivers.Any(r => typeof(StubFragmentReceiverTypeA) == r.GetType()));
            Assert.IsTrue(receivers.Any(r => typeof(StubFragmentReceiverTypeB) == r.GetType()));
        }

        [TestMethod]
        public void GetReceivers_WithIdempotentReceiver_GetsExpectedReceivers()
        {
            // Act
            var receivers = EndpointConfiguration.GetConfiguredReceivers("http://test/graph/idempotent");

            // Assert
            Assert.AreEqual(2, receivers.Count());
            Assert.IsTrue(receivers.Any(r => typeof(StubFragmentReceiverTypeA) == r.GetType()));
            Assert.IsTrue(receivers.Any(r => typeof(IdempotentFragmentReceiverWrapper) == r.GetType()));
        }

        [TestMethod]
        public void GetReceivers_ReceiverWithLabels_LabelsSetCorrectly()
        {
            // Act
            var receivers = EndpointConfiguration.GetConfiguredReceivers("http://test/graph/idempotent");

            // Assert
            Assert.AreEqual(2, receivers.Count());
            Assert.IsTrue(receivers.Any(r => typeof(StubFragmentReceiverTypeA) == r.GetType()));
            Assert.IsTrue(receivers.Any(r => typeof(IdempotentFragmentReceiverWrapper) == r.GetType()));
        }

        [TestMethod]
        public void GetReceivers_ReceiverWithLabels_LabelsSetCorrectlyOnReceiver()
        {
            // Act
            var receiver = EndpointConfiguration.GetConfiguredReceivers("http://test/graph")
                                .Single(r => r is StubFragmentReceiverTypeB);

            // Assert
            Assert.AreEqual(2, receiver.Labels.Count());
            Assert.IsTrue(receiver.Labels.Contains("XX1"));
            Assert.IsTrue(receiver.Labels.Contains("XX2"));
        }

        [TestMethod]
        public void Port_IsConfigured_GetsExpectedPort()
        {
            // Act
            var port = EndpointConfiguration.Port;

            // Assert
            Assert.AreEqual("9000", port);
        }

        [TestMethod]
        public void Addresses_IsConfigured_GetsExpectedAddresses()
        {
            // Arrange
            var machineName = Dns.GetHostName();
            var entry = Dns.GetHostEntry(machineName);

            // Act
            var adresses = EndpointConfiguration.Addresses.ToList();

            // Assert
            Assert.IsTrue(adresses.Count>=4);
            Assert.IsTrue(adresses.Any(a => a.Contains("localhost:")));
            Assert.IsTrue(adresses.Any(a => a.Contains("127.0.0.1:")));
            Assert.IsTrue(adresses.Any(a => a.Contains(machineName + ":")));
            Assert.IsTrue(adresses.Any(a => a.Contains(entry.HostName)));

            foreach (var adress in adresses)
            {
                Console.WriteLine(adress);
            }
        }

        [TestMethod]
        public void IsIpv6_ShouldBe_Is()
        {
            // Arrange
            var address = "2001:0:9d38:90d7:3ca1:5b7:53ec:75e4";

            // Act
            var isIpv6 = EndpointConfiguration.IsIpv6(address);

            // Assert
            Assert.IsTrue(isIpv6);
        }

        [TestMethod]
        public void IsIpv6_ShouldNotBe_IsNot()
        {
            // Arrange
            var address = "192.168.80.1";

            // Act
            var isIpv6 = EndpointConfiguration.IsIpv6(address);

            // Assert
            Assert.IsFalse(isIpv6);
        }
    }
}
