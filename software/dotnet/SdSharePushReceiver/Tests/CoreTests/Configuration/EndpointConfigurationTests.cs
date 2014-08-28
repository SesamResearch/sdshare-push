using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void BaseAddress_IsConfigured_GetsExpectedEndpointAddress()
        {
            // Act
            var baseAddress = EndpointConfiguration.BaseAddress;

            // Assert
            Assert.AreEqual("http://localhost:9000/", baseAddress);
        }
    }
}
