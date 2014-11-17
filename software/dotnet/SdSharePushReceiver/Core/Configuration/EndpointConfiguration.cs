using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using SdShare.Diagnostics;
using SdShare.Documentation;
using SdShare.Idempotency;

namespace SdShare.Configuration
{
    public static class EndpointConfiguration
    {
        private const string SectionName = "SdShareReceiverConfigurationSection";
        private const string AllKey = "##ALL##";
        private static readonly object SyncLock = new object();
        private static Dictionary<string, List<IFragmentReceiver>> _receivers;
        private static SdShareReceiverConfigurationSection _configSection;

        private static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(1);
        private const CacheMethod DefaultCacheMethod = Idempotency.CacheMethod.Memory;

        public static IEnumerable<IFragmentReceiver> GetConfiguredReceivers(string graphUri)
        {
            InitializeFromConfig();

            var recs = new List<IFragmentReceiver>();
            if (_receivers.ContainsKey(AllKey))
            {
                recs.AddRange(_receivers[AllKey]);
            }

            if (!string.IsNullOrWhiteSpace(graphUri) && _receivers.ContainsKey(graphUri))
            {
                recs.AddRange(_receivers[graphUri]);
            }

            return recs;
        }

        public static StatusDocumentation GetDocumentation()
        {
            return new StatusDocumentation
            {
                Endpoint = new EndpointDocumentation
                {
                    Port = int.Parse(Port),
                    Addresses = Addresses,
                    Receivers = GetReceiverDocumentations()
                },
                Diagnostics = new DiagnosticsDocumentation
                {
                    ErrorCount = DiagnosticData.ErrorCount,
                    RequestCount = DiagnosticData.RequestCount,
                    ResourceCount = DiagnosticData.ResourceCount,
                    StartTimeUtc = DiagnosticData.StartTimeUtc
                }
            };
        }

        public static string GetReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== SDShare Push Receiver Endpoint ===");
            sb.AppendFormat("Base address is: {0}\r\n", Port);
            sb.AppendLine("Configured receivers are:");
            foreach (ReceiverTypeElement receiver in _configSection.Receivers)
            {
                sb.AppendFormat("   {0} [handling graph: {1}]\r\n",
                    receiver.Name,
                    string.IsNullOrWhiteSpace(receiver.Graph) ? "ALL" : receiver.Graph);
            }

            return sb.ToString();
        }

        public static string Port
        {
            get
            {
                InitializeFromConfig();
                return _configSection.Port;
            }
        }

        public static IEnumerable<string> Addresses
        {
            get
            {
                var entry = Dns.GetHostEntry(Dns.GetHostName());

                string localIP = "?";
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        localIP = ip.ToString();
                    }
                }

                var urls = new List<string>
                {
                    string.Format("http://localhost:{0}/", Port),
                    string.Format("http://127.0.0.1:{0}/", Port),
                    string.Format("http://{0}:{1}/", Dns.GetHostName(),Port),
                    string.Format("http://{0}:{1}/", entry.HostName, Port)
               };

                urls.AddRange(host.AddressList.Where(a => !a.ToString().Contains("::")).Select(a => string.Format("http://{0}:{1}/", a, Port)));

                return urls;
            }
        }

        private static void InitializeFromConfig()
        {
            if (_receivers != null)
            {
                return;
            }

            lock (SyncLock)
            {
                if (_receivers != null)
                {
                    return;
                }

                _receivers = ReadReceivers();
            }
        }

        private static Dictionary<string, List<IFragmentReceiver>> ReadReceivers()
        {
            _configSection = (SdShareReceiverConfigurationSection)ConfigurationManager.GetSection(SectionName);

            return _configSection.Receivers.Cast<ReceiverTypeElement>().ToList()
                .Aggregate(
                    new Dictionary<string, List<IFragmentReceiver>>(),
                    (map, each) =>
                    {
                        List<IFragmentReceiver> list;
                        var key = string.IsNullOrWhiteSpace(each.Graph) ? AllKey : each.Graph;
                        if (map.ContainsKey(key))
                        {
                            list = map[key];
                        }
                        else
                        {
                            list = new List<IFragmentReceiver>();
                            map.Add(key, list);
                        }

                        var type = Type.GetType(each.Type);
                        if (type == null)
                        {
                            throw new ConfigurationErrorsException(string.Format("Unknown type: {0}", each.Type));
                        }

                        var receiver = (IFragmentReceiver) Activator.CreateInstance(type);
                        if (each.Idempotent)
                        {
                            var cacheExpiration = GetExpiration(each);
                            var cacheMethod = GetCacheMethod(each);
                            var wrapper = new IdempotentFragmentReceiverWrapper(receiver, cacheExpiration, cacheMethod);
                            list.Add(wrapper);
                        }
                        else
                        {
                            list.Add(receiver);
                        }

                        
                        return map;
                    });
        }

        private static TimeSpan GetExpiration(ReceiverTypeElement configElement)
        {
            return string.IsNullOrWhiteSpace(configElement.IdempotencyCacheExpirationSpan) 
                ? DefaultExpiration 
                : TimeSpan.Parse(configElement.IdempotencyCacheExpirationSpan);
        }

        private static CacheMethod GetCacheMethod(ReceiverTypeElement configElement)
        {
            if (string.IsNullOrWhiteSpace(configElement.IdempotencyCacheStrategy))
            {
                return DefaultCacheMethod;
            }

            return configElement.IdempotencyCacheStrategy.ToLower().StartsWith("memory")
                ? CacheMethod.Memory
                : CacheMethod.File;
        }

        private static IEnumerable<ReceiverDocumentation> GetReceiverDocumentations()
        {
            return _receivers.Aggregate(
                new List<ReceiverDocumentation>(),
                (accumulator, eachPair) =>
                {
                    foreach (var receiver in eachPair.Value)
                    {
                        var rcr = receiver;
                        var doc = new ReceiverDocumentation {Graph = eachPair.Key};
                        if (typeof (IdempotentFragmentReceiverWrapper) == receiver.GetType())
                        {
                            var wrapper = (IdempotentFragmentReceiverWrapper) receiver;
                            rcr = wrapper.WrappedReceiver;
                            doc.Idempotent = true;
                            doc.IdempotencyCacheMethod = wrapper.CacheMethod.ToString();
                            doc.IdempotencyCacheExpiration = wrapper.Expiration.ToString();
                        }

                        doc.Type = rcr.GetType().AssemblyQualifiedName;
                        accumulator.Add(doc);
                    }

                    return accumulator;
                });

        }
    }
}
