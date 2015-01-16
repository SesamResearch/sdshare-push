using System;
using System.Collections.Generic;
using System.Linq;
using SdShare.Metadata;

namespace SdShare.Documentation
{
    public static class ExceptionLogs
    {
        private static readonly Dictionary<DateTime, ExceptionLogInfo> _exceptions = new Dictionary<DateTime, ExceptionLogInfo>();
        private static readonly object _syncLock = new object();

        public static IEnumerable<ExceptionLogInfo> GetExceptions()
        {
            return GetExceptions(DateTime.MinValue);
        }

        public static IEnumerable<ExceptionLogInfo> GetExceptions(DateTime after)
        {
            lock (_syncLock)
            {
                var parser = new ExceptionLogParser();
                foreach (var logInfo in parser.Parse().Where(logInfo => !_exceptions.ContainsKey(logInfo.TimeUtc)))
                {
                    _exceptions[logInfo.TimeUtc] = logInfo;
                }
            }

            return _exceptions.Where(pair => pair.Key > after).Select(pair => pair.Value);
        }

        public static IEnumerable<IMetadata> GetExceptionMetadata(DateTime since)
        {
            return GetExceptions(since).Select(ex => new ExceptionLogMetaElement(ex));
        } 
    }
}
