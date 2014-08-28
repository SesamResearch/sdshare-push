using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SdShare.Configuration
{
    public static class ReceiverManager
    {
        private const string SectionName = "SdShareReceiverConfigurationSection";
        private const string AllKey = "##ALL##";
        private static readonly object SyncLock = new object();
        private static Dictionary<string, List<IFragmentReceiver>> Receivers;

        public static IEnumerable<IFragmentReceiver> GetReceivers(string graphUri)
        {
            InitializeFromConfig();

            var recs = new List<IFragmentReceiver>();
            if (Receivers.ContainsKey(AllKey))
            {
                recs.AddRange(Receivers[AllKey]);
            }

            if (!string.IsNullOrWhiteSpace(graphUri) && Receivers.ContainsKey(graphUri))
            {
                recs.AddRange(Receivers[graphUri]);
            }

            return recs;
        }

        private static void InitializeFromConfig()
        {
            if (Receivers != null)
            {
                return;
            }

            lock (SyncLock)
            {
                if (Receivers != null)
                {
                    return;
                }

                Receivers = ReadReceivers();
            }
        }

        private static Dictionary<string, List<IFragmentReceiver>> ReadReceivers()
        {
            var section = (SdShareReceiverConfigurationSection)ConfigurationManager.GetSection(SectionName);

            return section.Receivers.Cast<ReceiverTypeElement>().ToList()
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

                        list.Add((IFragmentReceiver)Activator.CreateInstance(Type.GetType(each.Type)));
                        return map;
                    });
        }
    }
}
