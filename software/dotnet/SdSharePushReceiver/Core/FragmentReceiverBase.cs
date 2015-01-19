using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Mindscape.Raygun4Net;
using NetTriple;
using NLog;
using SdShare.Exceptions;

namespace SdShare
{
    public abstract class FragmentReceiverBase : IFragmentReceiver
    {
        private Logger _logger;

        private readonly string _raygunApiKey = ConfigurationManager.AppSettings["Raygun.Api.Key"];
        private readonly RaygunClient _raygunClient = new RaygunClient(ConfigurationManager.AppSettings["Raygun.Api.Key"]);
        private readonly List<string> _errorTags = new List<string>(); 

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
                OnException(e, resources, graphUri, payload);

                SendToRaygun(e, resources, graphUri);

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

                ExceptionWriter.Write(e, resources, payload);

                throw;
            }
        }

        public IEnumerable<string> Labels { get; set; }

        protected abstract Type LoggerNamespaceType { get; }

        protected Logger Logger
        {
            get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger(LoggerNamespaceType)); }
        }

        protected abstract bool SupportsBatching { get; }

        protected abstract void DeleteResource(string resource);

        protected abstract void ReceiveCore(IEnumerable<string> resources, string graphUri, string payload);

        protected abstract void OnException(Exception exception, IEnumerable<string> resources, string graphUri, string payload);

        protected IEnumerable<object> GetObjects(string payload)
        {
            var triples = payload.ToTriplesFromNTriples().ToList();
            var unknowns = new List<string>();
            var objects = triples.ToObjects(unknowns);
            ExceptionWriter.WriteOrphans(unknowns);
            return objects;
        }

        protected IDictionary<string, object> GetInflationContext(string payload)
        {
            var triples = payload.ToTriplesFromNTriples().ToList();
            var unknowns = new List<string>();
            var context = triples.GetInflationContext(unknowns);
            ExceptionWriter.WriteOrphans(unknowns);
            return context.GetAllSubjectEntityPair();
        }

        protected T GetObject<T>(string payload)
        {
            var triples = payload.ToTriplesFromNTriples().ToList();
            var unknowns = new List<string>();
            var obj = triples.ToObject<T>(unknowns);
            ExceptionWriter.WriteOrphans(unknowns);
            return obj;
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

        private void SendToRaygun(Exception exception, IEnumerable<string> resources, string graphUri)
        {
            if (string.IsNullOrWhiteSpace(_raygunApiKey))
            {
                return;
            }

            if (_errorTags.Count == 0)
            {
                _errorTags.AddRange(Labels);

                if (ConfigurationManager.AppSettings.AllKeys.Contains("ServiceName"))
                {
                    _errorTags.Add(ConfigurationManager.AppSettings["ServiceName"]);
                }
            }

            var rscrs = resources == null || resources.Count() == 0
                ? "MISSING"
                : resources.Aggregate(new StringBuilder(), (sb, r) =>
                {
                    sb.Append(r);
                    sb.Append(";");
                    return sb;
                }).ToString();

            var customData = new Dictionary<string, string>()
            {
                {"resources", rscrs},
                {"graph", graphUri ?? "NONE"}
            };

            _raygunClient.Send(exception, _errorTags, customData);
        }
    }
}
