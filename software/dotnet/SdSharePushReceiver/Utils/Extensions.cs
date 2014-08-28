using System.IO;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace SdShare
{
    public static class Extensions
    {
        public static IGraph ToGraph(this string payload)
        {
            var g = new Graph();
            var parser = new NTriplesParser();
            using (var reader = new StringReader(payload))
            {
                parser.Load(g, reader);
            }

            return g;
        }
    }
}
