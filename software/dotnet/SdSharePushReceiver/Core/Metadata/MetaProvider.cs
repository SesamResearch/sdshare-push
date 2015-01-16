using System;
using System.Collections.Generic;
using System.Linq;
using NetTriple;
using NetTriple.Fluency;
using SdShare.Diagnostics;
using SdShare.Documentation;

namespace SdShare.Metadata
{
    public static class MetaProvider
    {
        private static readonly List<Func<DateTime, IEnumerable<IMetadata>>> MetadataGetters = new List<Func<DateTime, IEnumerable<IMetadata>>>
        {
            DiagnosticData.GetDiagnosticDataInTime,
            ExceptionLogs.GetExceptionMetadata
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

        public static IEnumerable<IBuiltTransform> GetTransforms()
        {
            return new List<IBuiltTransform>
            {
                BuildTransform.For<DiagnosticDataInTime>("http://psi.hafslund.no/sesam/meta/Diagnostics")
                    .Subject(d => d.Id, "http://psi.hafslund.no/sesam/meta/diagnostics/{0}")
                    .WithPropertyPredicateBase("http://psi.hafslund.no/sesam/meta/diagnostics")
                    .Prop(d => d.Id, "/id")
                    .Prop(d => d.TimeUtc, "/timeUtc")
                    .Prop(d => d.RequestCount, "/requestCount")
                    .Prop(d => d.ResourceCount, "/resourceCount")
                    .Prop(d => d.ErrorCount, "/errorCount")
                    .Prop(d => d.Flows, "/flows"),

                BuildTransform.For<ExceptionLogMetaElement>("http://psi.hafslund.no/sesam/meta/Exception")
                    .Subject(d => d.Id, "http://psi.hafslund.no/sesam/meta/exception/{0}")
                    .WithPropertyPredicateBase("http://psi.hafslund.no/sesam/meta/exception")
                    .Prop(e => e.Id, "/id")
                    .Prop(e => e.TimeUtc, "/timeUtc")
                    .Prop(e => e.Resources, "/resources")
                    .Prop(e => e.Payload, "/payload")
                    .Prop(e => e.ExceptionDetails, "/exceptionDetails")
                    .Prop(e => e.TargetPayload, "/targetPayload")
                    .Prop(e => e.Flows, "/flows")
            };
        }
    }
}
