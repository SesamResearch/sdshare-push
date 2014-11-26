using System.Collections.Generic;

namespace SdShare.Documentation
{
    public class ReceiverDocumentation
    {
        public string Name { get; set; }
        public string Graph { get; set; }
        public string Type { get; set; }
        public bool Idempotent { get; set; }
        public string IdempotencyCacheExpiration { get; set; }
        public string IdempotencyCacheMethod { get; set; }
        public IEnumerable<string> Labels { get; set; }
    }
}
