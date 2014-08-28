using SdShare;

namespace TestUtils
{
    public class StubFragmentReceiverTypeA : IFragmentReceiver
    {
        public void Receive(string resourceUri, string graphUri, string payload)
        {
            ReceivedFragmentInvocations.IncTypeA();
        }
    }
}
