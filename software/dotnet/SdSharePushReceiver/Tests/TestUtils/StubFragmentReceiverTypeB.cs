using SdShare;

namespace TestUtils
{
    public class StubFragmentReceiverTypeB : IFragmentReceiver
    {
        public void Receive(string resourceUri, string graphUri, string payload)
        {
            ReceivedFragmentInvocations.IncTypeB();
        }
    }
}
