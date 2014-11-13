using System.Collections.Generic;
using SdShare;

namespace TestUtils
{
    public class StubFragmentReceiverTypeA : IFragmentReceiver
    {
        public void Receive(IEnumerable<string> resources, string graphUri, string payload)
        {
            ReceivedFragmentInvocations.IncTypeA();
        }
    }
}
