using System;
using SdShare;

namespace TestHost
{
    public class MyReceiver : IFragmentReceiver
    {
        public void Receive(string resourceUri, string graphUri, string payload)
        {
            Console.WriteLine("Handling incoming request.");
            Console.WriteLine("Resource: {0}", resourceUri);
            Console.WriteLine("Graph: {0}", graphUri);
            Console.WriteLine("Payload: {0}", payload.ToWashedTriplePayload());
            Console.WriteLine("==== ==== ==== ==== ==== ==== ==== ====");
        }
    }
}
