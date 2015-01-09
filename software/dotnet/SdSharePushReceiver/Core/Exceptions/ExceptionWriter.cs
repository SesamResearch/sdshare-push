using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace SdShare.Exceptions
{
    public static class ExceptionWriter
    {
        private static readonly Logger ExceptionLogger = LogManager.GetLogger("SdShare.PushReceiver.Exceptions");
        private static readonly Logger OrphanLogger = LogManager.GetLogger("SdShare.PushReceiver.Orphans");

        public static void Write(Exception exception, IEnumerable<string> resources, string payload)
        {
            var rsrsc = resources == null
                    ? "NULL"
                    : resources.Aggregate(new StringBuilder(), (sb, r) =>
                    {
                        sb.AppendFormat("resource={0}& ", r);
                        return sb;
                    }).ToString();

            ExceptionLogger.Error("STARTEXCEPTION");

            if (resources != null)
            {
                ExceptionLogger.Error("STARTRESOURCES");
                foreach (var resource in resources)
                {
                    ExceptionLogger.Error(resource);
                }

                ExceptionLogger.Error("ENDRESOURCES");
            }

            ExceptionLogger.Error("STARTPAYLOAD");
            ExceptionLogger.Error(payload ?? string.Empty);
            ExceptionLogger.Error("ENDPAYLOAD");

            ExceptionLogger.Error("STARTDETAILS");
            ExceptionLogger.ErrorException(rsrsc, exception);
            ExceptionLogger.Error("ENDDETAILS");

            ExceptionLogger.Error("ENDEXCEPTION");
        }

        public static void WriteOrphans(IEnumerable<string> orphans)
        {
            if (orphans == null)
            {
                return;
            }

            foreach (var orphan in orphans)
            {
                OrphanLogger.Warn(orphan);
            }
        }
    }
}
