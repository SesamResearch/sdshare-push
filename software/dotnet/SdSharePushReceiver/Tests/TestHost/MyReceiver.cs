using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SdShare;

namespace TestHost
{
    public class MyReceiver : FragmentReceiverBase
    {
        protected override Type LoggerNamespaceType
        {
            get { return GetType(); }
        }

        protected override bool SupportsBatching
        {
            get { return true; }
        }

        protected override void DeleteResource(string resource)
        {
        }

        protected override void ReceiveCore(IEnumerable<string> resources, string graphUri, string payload)
        {
            Console.WriteLine("Handling incoming request.");
            Console.WriteLine("Resource: {0}", resources.Aggregate(new StringBuilder(), (sb, r) =>
            {
                sb.AppendFormat("{0} ", r);
                return sb;
            }));

            Console.WriteLine("Graph: {0}", graphUri);

            if (string.IsNullOrWhiteSpace(payload))
            {
                Console.WriteLine("I've been asked to crash, so I will");
                throw new InvalidOperationException("Crash by request");
            }

            var washedPayload = payload.ToWashedTriplePayload();
            if (string.IsNullOrWhiteSpace(washedPayload))
            {
                Console.WriteLine("Raw payload: {0}", payload);
            }
            else
            {
                Console.WriteLine("Payload: {0}", washedPayload);
            }

            Console.WriteLine("==== ==== ==== ==== ==== ==== ==== ====");
        }

        protected override void OnException(Exception exception, IEnumerable<string> resources, string graphUri, string payload)
        {
        }
    }
}
