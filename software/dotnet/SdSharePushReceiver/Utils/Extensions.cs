using System;
using System.IO;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// When testing with tools directly in chrome (such as Postman), the
        /// payload might be wrapped in text, for instance starting with this:
        /// ------WebKitFormBoundarysoh7g9XuBMxTzC9M
        /// 
        /// This method assumes that the actual payload consists of triples, each
        /// triple on a line, and all text that does not look loke a triple is
        /// removed.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string ToWashedTriplePayload(this string payload)
        {
            if (string.IsNullOrEmpty(payload))
            {
                return payload;
            }

            return payload.Split(new[] {"\r\n", "\n"}, StringSplitOptions.None)
                .Aggregate(
                    new StringBuilder(),
                    (sb, line) =>
                    {
                        if (line.TrimStart().StartsWith("<"))
                        {
                            sb.AppendLine(line);
                        }

                        return sb;
                    }).ToString();
        }
    }
}
