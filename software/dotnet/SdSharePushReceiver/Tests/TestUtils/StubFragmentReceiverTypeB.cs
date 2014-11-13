using System.Collections.Generic;
using SdShare;

namespace TestUtils
{
    public class StubFragmentReceiverTypeB : IFragmentReceiver
    {
        public void Receive(IEnumerable<string> resources, string graphUri, string payload)
        {
            ReceivedFragmentInvocations.IncTypeB();
        }
    }
}
