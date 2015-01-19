using System;
using System.Collections.Generic;
using System.Linq;
using NetTriple;
using NetTriple.Documentation;
using NetTriple.Emit;
using Newtonsoft.Json;
using SdShare.Configuration;
using SdShare.Metadata;

namespace SdShare.Documentation
{
    public class DocumentationMetadataElement : IMetadata
    {
        public DocumentationMetadataElement()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
            Version = EndpointConfiguration.GetFileVersion();
        }

        public DocumentationMetadataElement(TypeTransformDocumentation doc, DateTime timeUtc) : this()
        {
            TimeUtc = timeUtc;
            RdfType = doc.RdfType;
            DotNetType = doc.Type;
            TargetNamespace = doc.XmlNamespace;
            Flows = EndpointConfiguration.Flows;
            if (doc.Properties != null && doc.Properties.Any())
            {
                var json = JsonConvert.SerializeObject(doc.Properties);
                Predicates = json.Compress();
            }
        }

        public string Id { get; set; }
        public DateTime TimeUtc { get; private set; }

        public string Version { get; private set; }
        public string RdfType { get; private set; }
        public string DotNetType { get; private set; }
        public string TargetNamespace { get; private set; }
        public string Predicates { get; private set; }
        public string Flows { get; private set; }

        public IEnumerable<Triple> Triples
        {
            get
            {
                var subject = string.Format("<http://psi.hafslund.no/sesam/meta/mapping/{0}>", Id);
                var triples = new List<Triple>
                {
                    new Triple{Subject = subject, Predicate = "<http://www.w3.org/1999/02/22-rdf-syntax-ns#type>", Object = "<http://psi.hafslund.no/sesam/meta/Mapping>"},
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/mapping/id>", Object = Id.ToTripleObject()}, 
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/mapping/timeUtc>", Object = TimeUtc.ToTripleObject()}, 
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/mapping/version>", Object = Version.ToTripleObject()}, 
                    new Triple{Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/mapping/dotNetType>", Object = DotNetType.ToTripleObject()}, 
                };

                if (!string.IsNullOrWhiteSpace(TargetNamespace))
                {
                    triples.Add(new Triple { Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/mapping/targetNamespace>", Object = TargetNamespace.ToTripleObject() });
                }

                if (!string.IsNullOrWhiteSpace(Predicates))
                {
                    triples.Add(new Triple { Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/mapping/predicates>", Object = Predicates.ToTripleObject() });
                }

                if (!string.IsNullOrWhiteSpace(RdfType))
                {
                    triples.Add(new Triple { Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/mapping/rdfType>", Object = RdfType.ToTripleObject() });
                }

                if (!string.IsNullOrWhiteSpace(Flows))
                {
                    triples.Add(new Triple { Subject = subject, Predicate = "<http://psi.hafslund.no/sesam/meta/mapping/flows>", Object = Flows.ToTripleObject() });
                }

                return triples;
            }
        }

        public string DocumentationJson { get; set; }
    }
}
