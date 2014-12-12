using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SdShare.Documentation;

namespace CoreTests.Documentation
{
    [TestClass]
    public class ExceptionLogParserTests
    {
        [TestMethod]
        public void Parse_ValidLog_ParsesAsExpected()
        {
            // Arrange
            var parser = new ExceptionLogParser();

            // Act
            var exceptions = parser.Parse(GetExceptionLines());

            // Assert
            Assert.AreEqual(3, exceptions.Count());
        }

        [TestMethod]
        public void HowDoesDateTimeParseWork()
        {
            // Arrange
            var dts = "2014-12-05 12:19:09.0743";

            // Act
            var dt = DateTime.Parse(dts);

            // Assert
            Assert.AreEqual(2014, dt.Year);
            Assert.AreEqual(12, dt.Month);
            Assert.AreEqual(5, dt.Day);
            Assert.AreEqual(12, dt.Hour);
            Assert.AreEqual(19, dt.Minute);
            Assert.AreEqual(9, dt.Second);
            Assert.AreEqual(74, dt.Millisecond);
        }

        private IList<string> GetExceptionLines()
        {
            return Resources.ExceptionLog.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}
