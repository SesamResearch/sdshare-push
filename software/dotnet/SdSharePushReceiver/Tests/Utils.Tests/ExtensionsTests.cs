using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SdShare;
using VDS.RDF;

namespace Utils.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void ToGraph_WithValidTripls_ReturnsExectedGraph()
        {
            // Act
            var graph = TestResources.Triples.ToGraph();

            // Assert
            Assert.AreEqual(2, graph.Nodes.Count(n => NodeType.Uri == n.NodeType));
        }

        [TestMethod]
        public void ToWashedTriplePayload_WithWebkitPayload_ReturnsJustTheTriples()
        {
            // Arrange
            var payload = TestResources.WebKitPayload;

            // Act
            var washed = payload.ToWashedTriplePayload();

            // Assert
            Assert.IsTrue(washed.Length < payload.Length);

            var triples = washed.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            Assert.AreEqual(3, triples.Count());
            Assert.IsTrue(triples.All(triple => triple.TrimStart().StartsWith("<http://sdshare.com/unittesting/order/1>")));
        }

        [TestMethod]
        public void ToWashedTriplePayload_WithNull_ReturnsNull()
        {
            // Arrange
            string payload = null;

            // Act
            var washed = payload.ToWashedTriplePayload();

            // Assert
            Assert.IsNull(washed);
        }
    }
}
