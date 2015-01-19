using System;
using System.Collections.Generic;
using System.Linq;
using SdShare.Diagnostics;
using SdShare.Documentation;

namespace SdShare.Metadata
{
    public static class MetaProvider
    {
        private static readonly List<Func<DateTime, IEnumerable<IMetadata>>> MetadataGetters = new List<Func<DateTime, IEnumerable<IMetadata>>>
        {
            DiagnosticData.GetDiagnosticDataInTime,
            ExceptionLogs.GetExceptionMetadata,
            DocumentationMetadata.GetMappingsData
        }; 

        public static string GetChanges(DateTime since)
        {
            var metadata = MetadataGetters.Aggregate(
                new List<IMetadata>(),
                (accum, func) =>
                {
                    var md = func.Invoke(since);
                    if (md != null)
                    {
                        accum.AddRange(md);
                    }

                    return accum;
                });

            return new MetadataFragmentWriter().GetFragments(metadata);
        }
    }
}
