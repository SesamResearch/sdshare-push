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
        private readonly Logger _exceptionLogger = LogManager.GetLogger("SdShare.PushReceiver.Exceptions");
        private readonly Dictionary<string, string> _targetPayloadMap = new Dictionary<string, string>();
        private readonly object _syncLock = new object();

        public void Receive(IEnumerable<string> resources, string graphUri, string payload)
        {
            var invocationKey = Guid.NewGuid().ToString();

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
                    ReceiveCore(invocationKey, rsrcs, graphUri, payload);
                }
            }
            catch (Exception e)
            {
                OnException(e);

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

                _exceptionLogger.Error("STARTEXCEPTION");

                if (resources != null)
                {
                    _exceptionLogger.Error("STARTRESOURCES");
                    foreach (var resource in resources)
                    {
                        _exceptionLogger.Error(resource);
                    }

                    _exceptionLogger.Error("ENDRESOURCES");
                }

                _exceptionLogger.Error("STARTPAYLOAD");
                _exceptionLogger.Error(payload);
                _exceptionLogger.Error("ENDPAYLOAD");

                var targetPayload = GetReportedTargetPayload(invocationKey);
                if (!string.IsNullOrWhiteSpace(targetPayload))
                {
                    _exceptionLogger.Error("STARTTARGETPAYLOAD");
                    _exceptionLogger.Error(targetPayload);
                    _exceptionLogger.Error("ENDTARGETPAYLOAD");
                }

                _exceptionLogger.Error("STARTDETAILS");
                _exceptionLogger.ErrorException(rsrsc, e);
                _exceptionLogger.Error("ENDDETAILS");

                _exceptionLogger.Error("ENDEXCEPTION");

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

        protected abstract void ReceiveCore(string invocationKey, IEnumerable<string> resources, string graphUri, string payload);

        protected abstract void OnException(Exception exception);

        protected void ReportTargetPayloadOnError(string invocationKey, string targetPayload)
        {
            if (string.IsNullOrWhiteSpace(invocationKey) || string.IsNullOrWhiteSpace(targetPayload))
            {
                return;
            }

            lock (_syncLock)
            {
                _targetPayloadMap[invocationKey] = targetPayload;
            }
        }

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

        private string GetReportedTargetPayload(string invocationKey)
        {
            if (!_targetPayloadMap.ContainsKey(invocationKey))
            {
                return null;
            }

            var payload = _targetPayloadMap[invocationKey];
            lock (_syncLock)
            {
                _targetPayloadMap.Remove(invocationKey);
            }

            return payload;
        }
    }
}
