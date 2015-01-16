using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SdShare;

namespace CoreTests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void Zip_HappyDays_CompressesAsExpected()
        {
            // Act
            var compressed = Resources.Triples.Compress();

            Console.WriteLine(compressed);
            Console.WriteLine("============================================");
            Console.WriteLine(compressed.Decompress());
        }
    }
}
