using System;
using System.Threading;

namespace SdShare.Diagnostics
{
    public static class DiagnosticData
    {
        public static readonly DateTime StartTimeUtc = DateTime.UtcNow;

        private static int _requestCount = 0;
        private static int _resourceCount = 0;
        private static int _errorCount = 0;

        public static int RequestCount { get { return _requestCount; } }
        public static int ResourceCount { get { return _resourceCount; } }
        public static int ErrorCount { get { return _errorCount; } }


        public static void IncRequests()
        {
            Interlocked.Increment(ref _requestCount);
        }

        public static void IncResources(int nr = 1)
        {
            for (int i = 0; i < nr; i++)
            {
                Interlocked.Increment(ref _resourceCount);
            }
        }

        public static void IncErrors()
        {
            Interlocked.Increment(ref _errorCount);
        }
    }
}
