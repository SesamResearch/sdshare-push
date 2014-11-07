using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SdShare.Configuration;
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
            Assert.AreEqual(4, adresses.Count);
            Assert.IsTrue(adresses.Any(a => a.Contains("localhost:")));
            Assert.IsTrue(adresses.Any(a => a.Contains("127.0.0.1:")));
            Assert.IsTrue(adresses.Any(a => a.Contains(machineName + ":")));
            Assert.IsTrue(adresses.Any(a => a.Contains(entry.HostName)));
        }
    }
}
