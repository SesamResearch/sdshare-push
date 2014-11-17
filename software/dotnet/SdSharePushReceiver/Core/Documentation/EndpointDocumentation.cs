using System.Collections.Generic;

namespace SdShare.Documentation
{
    public class EndpointDocumentation
    {
        public int Port { get; set; }
        public IEnumerable<string> Addresses { get; set; }
        public IEnumerable<ReceiverDocumentation> Receivers { get; set; } 
    }
}
