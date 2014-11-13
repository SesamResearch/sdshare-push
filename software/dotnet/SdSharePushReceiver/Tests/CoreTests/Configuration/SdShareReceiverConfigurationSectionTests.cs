using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SdShare.Configuration;

namespace CoreTests.Configuration
{
    [TestClass]
    public class SdShareReceiverConfigurationSectionTests
    {
        [TestMethod]
        public void GetSection_HappyDays_GetsSection()
        {
            // Act
            var section = (SdShareReceiverConfigurationSection)ConfigurationManager.GetSection("SdShareReceiverConfigurationSection");

            // Assert
            Assert.IsNotNull(section);
        }

        [TestMethod]
        public void GetReceivers_InSection_ReturnsExpectedReceivers()
        {
            // Arrange
            var section = (SdShareReceiverConfigurationSection)ConfigurationManager.GetSection("SdShareReceiverConfigurationSection");

            // Act
            var elements = section.Receivers.Cast<ReceiverTypeElement>().ToList();

            // Assert
            Assert.AreEqual(3, elements.Count);
        }

        [TestMethod]
        public void GetSection_HappyDays_HasExpectedPort()
        {
            // Arrange
            var section = (SdShareReceiverConfigurationSection)ConfigurationManager.GetSection("SdShareReceiverConfigurationSection");

            // Act
            var port = section.Port;

            // Assert
            Assert.AreEqual("9000", port);
        }

        [TestMethod]
        public void GetReceivers_WithIdempotency_HasExpectedIdempotencyData()
        {
            // Arrange
            var section = (SdShareReceiverConfigurationSection)ConfigurationManager.GetSection("SdShareReceiverConfigurationSection");

            // Act
            var idempotent = section.Receivers.Cast<ReceiverTypeElement>().Single(el => el.Name == "AIdempotent");
            var notIdempotent = section.Receivers.Cast<ReceiverTypeElement>().Single(el => el.Name == "B");

            // Assert
            Assert.IsTrue(idempotent.Idempotent);
            Assert.AreEqual("memory", idempotent.IdempotencyCacheStrategy);
            Assert.AreEqual("02:02:01", idempotent.IdempotencyCacheExpirationSpan);
            Assert.IsFalse(notIdempotent.Idempotent);
        }
    }
}
