using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SdShare.Configuration
{
    public static class EndpointConfiguration
    {
        private const string SectionName = "SdShareReceiverConfigurationSection";
        private const string AllKey = "##ALL##";
        private static readonly object SyncLock = new object();
        private static Dictionary<string, List<IFragmentReceiver>> _receivers;
        private static SdShareReceiverConfigurationSection _configSection;

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

        public static string GetReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== SDShare Push Receiver Endpoint ===");
            sb.AppendFormat("Base address is: {0}\r\n", BaseAddress);
            sb.AppendLine("Configured receivers are:");
            foreach (ReceiverTypeElement receiver in _configSection.Receivers)
            {
                sb.AppendFormat("   {0} [handling graph: {1}]\r\n",
                    receiver.Name,
                    string.IsNullOrWhiteSpace(receiver.Graph) ? "ALL" : receiver.Graph);
            }

            return sb.ToString();
        }

        public static string BaseAddress
        {
            get
            {
                InitializeFromConfig();
                return _configSection.BaseAddress;
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

                        list.Add((IFragmentReceiver)Activator.CreateInstance(type));
                        return map;
                    });
        }
    }
}
