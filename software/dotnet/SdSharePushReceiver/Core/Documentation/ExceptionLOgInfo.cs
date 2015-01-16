using System;
using System.Collections.Generic;
using NetTriple;
using SdShare.Metadata;

namespace SdShare.Documentation
{
    public class ExceptionLogInfo
    {
        public DateTime TimeUtc { get; set; }
        public IEnumerable<string> Resources { get; set; }
        public string Payload { get; set; }
        public string ExceptionDetails { get; set; }
        public string TargetPayload { get; set; }
        public string Flows { get; set; }
    }
}
