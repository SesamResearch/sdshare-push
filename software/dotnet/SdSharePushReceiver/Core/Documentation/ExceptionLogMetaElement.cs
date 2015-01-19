using System;
using System.Collections.Generic;
using NetTriple;
using NetTriple.Emit;
using SdShare.Configuration;
using SdShare.Metadata;

namespace SdShare.Documentation
{
    public class ExceptionLogMetaElement : IMetadata
    {
        public ExceptionLogMetaElement()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
        }

        public ExceptionLogMetaElement(ExceptionLogInfo ex) : this()
        {
            TimeUtc = ex.TimeUtc;
            Resources = ex.Resources.ToConcatenatedString();
            Payload = ex.Payload.Compress();
            ExceptionDetails = ex.ExceptionDetails.Compress();
            TargetPayload = ex.TargetPayload.Compress();
            Flows = EndpointConfiguration.Flows; ;
        }

        public string Id { get; set; }
        public DateTime TimeUtc { get; set; }
        public string Resources { get; set; }
        public string Payload { get; set; }
        public string ExceptionDetails { get; set; }
        public string TargetPayload { get; set; }
        public string Flows { get; set; }

        public IEnumerable<Triple> Triples
        {
            get
            {
                var subject = string.Format("<http://psi.hafslund.no/sesam/meta/exception/{0}>", Id);
                var triples = new List<Triple>
                {
                    new Triple{Subject = subject, Predicate = "<http://www.w3.org/1999/02/22-rdf-syntax-ns#type>", Object = "<http://psi.hafslund.no/sesam/meta/Exception>"},
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/exception/id>", Object = Id.ToTripleObject()},
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/exception/timeUtc>", Object = TimeUtc.ToTripleObject()}
                };

                if (!string.IsNullOrWhiteSpace(Payload))
                {
                    triples.Add(new Triple { Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/exception/payload>", Object = Payload.ToTripleObject() });
                }

                if (!string.IsNullOrWhiteSpace(Resources))
                {
                    triples.Add(new Triple { Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/exception/resources>", Object = Resources.ToTripleObject() });
                }

                if (!string.IsNullOrWhiteSpace(ExceptionDetails))
                {
                    triples.Add(new Triple { Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/exception/exceptionDetails>", Object = ExceptionDetails.ToTripleObject() });
                }

                if (!string.IsNullOrWhiteSpace(TargetPayload))
                {
                    triples.Add(new Triple { Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/exception/targetPayload>", Object = TargetPayload.ToTripleObject() });
                }

                if (!string.IsNullOrWhiteSpace(Flows))
                {
                    triples.Add(new Triple { Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/exception/flows>", Object = Flows.ToTripleObject() });
                }

                return triples;
            }
        }
    }
}
