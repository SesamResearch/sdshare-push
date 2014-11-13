using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests.Idempotency
{
    [TestClass]
    public class HowDoesHashCodingWork
    {
        [TestMethod]
        public void GetHashCode()
        {
            Console.WriteLine(TimeSpan.Parse("02:02:01").ToString());
        }
    }
}
