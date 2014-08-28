using System;
using SdShare;

namespace TestHost
{
    public class MyReceiver : IFragmentReceiver
    {
        public void Receive(string resourceUri, string graphUri, string payload)
        {
            throw new NotImplementedException();
        }
    }
}
