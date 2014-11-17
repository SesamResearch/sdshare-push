using System;

namespace SdShare.Documentation
{
    public class DiagnosticsDocumentation
    {
        public DateTime StartTimeUtc { get; set; }
        public int RequestCount { get; set; }
        public int ResourceCount { get; set; }
        public int ErrorCount { get; set; }
    }
}
