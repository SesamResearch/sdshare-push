using System;
using System.Collections.Generic;

namespace SdShare.Documentation
{
    public class ExceptionLogInfo
    {
        public DateTime Time { get; set; }
        public IEnumerable<string> Resources { get; set; }
        public string Payload { get; set; }
        public string ExceptionDetails { get; set; }
    }
}
