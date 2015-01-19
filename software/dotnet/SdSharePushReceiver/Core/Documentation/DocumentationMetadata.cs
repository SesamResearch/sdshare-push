using System;
using System.Collections.Generic;
using System.Linq;
using NetTriple.Documentation;
using SdShare.Diagnostics;
using SdShare.Metadata;

namespace SdShare.Documentation
{
    public static class DocumentationMetadata
    {
        public static IEnumerable<IMetadata> GetMappingsData(DateTime since)
        {
            var st = DiagnosticData.StartTimeUtc;
            if (since > st)
            {
                return null;
            }

            return DocumentationFinder.GetTypeDocumentation().Select(d => new DocumentationMetadataElement(d, st));
        }
    }
}
