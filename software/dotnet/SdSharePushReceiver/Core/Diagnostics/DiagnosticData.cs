using System;
using System.Collections.Generic;
using System.Threading;
using SdShare.Configuration;
using SdShare.Metadata;

namespace SdShare.Diagnostics
{
    public static class DiagnosticData
    {
        public static readonly DateTime StartTimeUtc = DateTime.UtcNow;

        private static readonly object SyncLock = new object();
        private static DateTime _lastChange = DateTime.MinValue;

        private static int _requestCount = 0;
        private static int _resourceCount = 0;
        private static int _errorCount = 0;

        public static int RequestCount { get { return _requestCount; } }
        public static int ResourceCount { get { return _resourceCount; } }
        public static int ErrorCount { get { return _errorCount; } }

        public static IEnumerable<IMetadata> GetDiagnosticDataInTime(DateTime since)
        {
            if (_lastChange <= since)
            {
                return null;
            }

            return new List<IMetadata>
            {
                new DiagnosticDataInTime
                {
                    Id = _lastChange.Ticks.ToString(),
                    TimeUtc = _lastChange,
                    ErrorCount = _errorCount,
                    RequestCount = _requestCount,
                    ResourceCount = _resourceCount,
                    Flows = EndpointConfiguration.Flows
                }
            };
        }

        public static void IncRequests()
        {
            Interlocked.Increment(ref _requestCount);
            SetLastChange();
        }

        public static void IncResources(int nr = 1)
        {
            for (int i = 0; i < nr; i++)
            {
                Interlocked.Increment(ref _resourceCount);
            }

            SetLastChange();
        }

        public static void IncErrors()
        {
            Interlocked.Increment(ref _errorCount);
            SetLastChange();
        }

        private static void SetLastChange()
        {
            lock (SyncLock)
            {
                _lastChange = DateTime.UtcNow;
            }
        }
    }
}
