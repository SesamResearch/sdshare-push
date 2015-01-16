using System;
using System.Collections.Generic;
using NetTriple;
using NetTriple.Emit;
using SdShare.Metadata;

namespace SdShare.Diagnostics
{
    public class DiagnosticDataInTime : IMetadata
    {
        public string Id { get; set; }
        public DateTime TimeUtc { get; set; }
        public int RequestCount { get; set; }
        public int ResourceCount { get; set; }
        public int ErrorCount { get; set; }
        public string Flows { get; set; }

        public IEnumerable<Triple> Triples
        {
            get
            {
                var subject = string.Format("http://psi.hafslund.no/sesam/meta/diagnostics/{0}", Id);
                var triples = new List<Triple>
                {
                    new Triple{Subject = subject, Predicate = "<http://www.w3.org/1999/02/22-rdf-syntax-ns#type>", Object = "<http://psi.hafslund.no/sesam/meta/Diagnostics>"},
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/diagnostics/id>", Object = Id.ToTripleObject()}, 
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/diagnostics/timeUtc>", Object = TimeUtc.ToTripleObject()}, 
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/diagnostics/requestCount>", Object = RequestCount.ToTripleObject()}, 
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/diagnostics/resourceCount>", Object = ResourceCount.ToTripleObject()}, 
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/diagnostics/errorCount>", Object = ErrorCount.ToTripleObject()}, 
                };

                if (!string.IsNullOrWhiteSpace(Flows))
                {
                    triples.Add(new Triple { Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/diagnostics/flows>", Object = Flows.ToTripleObject() });
                }

                return triples;
            }
        }
    }
}
