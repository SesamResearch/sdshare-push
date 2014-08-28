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
    }
}
