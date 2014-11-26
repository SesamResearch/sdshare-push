using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace SdShare
{
    public abstract class FragmentReceiverBase : IFragmentReceiver
    {
        private Logger _logger;

        public void Receive(IEnumerable<string> resources, string graphUri, string payload)
        {
            try
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
            catch (Exception e)
            {
                var rsrsc = resources == null
                    ? "NULL"
                    : resources.Aggregate(new StringBuilder(), (sb, r) =>
                    {
                        sb.AppendFormat("resource={0}& ", r);
                        return sb;
                    }).ToString();

                Logger.Info("Error resources: " + rsrsc);
                Logger.Info("Error graph: " + graphUri ?? "NULL");
                Logger.Info(string.Format("Payload:\r\n{0}", payload));
                Logger.ErrorException("Exception details: ", e);
                throw;
            }
        }

        protected abstract Type LoggerNamespaceType { get; }

        protected Logger Logger
        {
            get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger(LoggerNamespaceType)); }
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
