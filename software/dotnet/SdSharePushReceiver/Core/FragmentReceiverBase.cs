using System;
using System.Collections.Generic;
using System.Linq;

namespace SdShare
{
    public abstract class FragmentReceiverBase : IFragmentReceiver
    {
        public void Receive(IEnumerable<string> resources, string graphUri, string payload)
        {
            var rsrcs = ValidateResources(resources);

            if (rsrcs.Count() > 1 && !SupportsBatching)
            {
                throw new InvalidOperationException("Batching not supported.");
            }

            if (string.IsNullOrEmpty(payload))
            {
                foreach (var resource in rsrcs)
                {
                    DeleteResource(resource);
                }
            }
            else
            {
                ValidateReceive(rsrcs, payload);
                ReceiveCore(rsrcs, graphUri, payload);
            }
        }

        protected abstract bool SupportsBatching { get; }

        protected abstract void DeleteResource(string resource);

        protected abstract void ReceiveCore(IEnumerable<string> resources, string graphUri, string payload);

        private void ValidateReceive(IEnumerable<string> resources, string payload)
        {
            var lines = payload.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var resource in resources.Where(resource => !lines.Any(line => line.StartsWith(string.Format("<{0}>", resource)))))
            {
                throw new InvalidOperationException(string.Format("No triples for resource {0}.", resource));
            }
        }

        private List<string> ValidateResources(IEnumerable<string> resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }

            var rsrcs = resources.ToList();

            if (rsrcs.Count == 0)
            {
                throw new ArgumentNullException("resources");
            }

            return rsrcs;
        }
    }
}
