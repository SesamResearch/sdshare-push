using System.Collections.Generic;
using NetTriple.Documentation;

namespace SdShare.Documentation
{
    public class StatusDocumentation
    {
        public EndpointDocumentation Endpoint { get; set; }
        public DiagnosticsDocumentation Diagnostics { get; set; }
        public IEnumerable<TypeTransformDocumentation> Transforms { get; set; }
        public string Version { get; set; }
    }
}
