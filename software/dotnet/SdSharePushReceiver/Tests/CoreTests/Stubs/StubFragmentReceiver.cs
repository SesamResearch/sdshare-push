using System.Collections.Generic;
using SdShare;

namespace CoreTests.Stubs
{
    public class StubFragmentReceiver : FragmentReceiverBase
    {
        public bool Batching { get; set; }
        public List<string> DeletedResources { get; set; }
        public bool ReceiveCoreReceived { get; private set; }

        public StubFragmentReceiver()
        {
            DeletedResources = new List<string>();
        }

        protected override bool SupportsBatching
        {
            get { return Batching; }
        }

        protected override void DeleteResource(string resource)
        {
            DeletedResources.Add(resource);
        }

        protected override void ReceiveCore(IEnumerable<string> resources, string graphUri, string payload)
        {
            ReceiveCoreReceived = true;
        }
    }
}
