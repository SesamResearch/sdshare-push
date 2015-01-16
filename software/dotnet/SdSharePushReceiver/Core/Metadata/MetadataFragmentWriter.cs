using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using NetTriple;

namespace SdShare.Metadata
{
    public class MetadataFragmentWriter
    {
        public string GetFragments(IEnumerable<IMetadata> metadata)
        {
            if (metadata == null || !metadata.Any())
            {
                return string.Empty;
            }

            string lastCheckSumLine = null;
            return metadata.Aggregate(
                new StringBuilder(),
                (sb, md) =>
                {
                    string nextCheckSum;
                    Append(md, sb, lastCheckSumLine, out nextCheckSum);
                    lastCheckSumLine = nextCheckSum;
                    return sb;
                }).ToString();
        }

        private void Append(IMetadata md, StringBuilder sb, string lastCheckSumLine, out string nextCheckSumLine)
        {
            var triples = md.Triples.ToList();
            var subject = triples.First().Subject;

            sb.AppendFormat("# {0} {1}\r\n", md.TimeUtc.ToString("u"), subject);
            foreach (var triple in triples)
            {
                sb.AppendFormat("{0} .\r\n", triple);
            }

            var hashable = string.IsNullOrWhiteSpace(lastCheckSumLine)
                ? sb.ToString()
                : string.Format("{0}\r\n{1}", lastCheckSumLine, sb.ToString());

            var bytes = new byte[hashable.Length * sizeof(char)];
            Buffer.BlockCopy(hashable.ToCharArray(), 0, bytes, 0, bytes.Length);
            var hash = Hash(bytes);

            nextCheckSumLine = string.Format("# RECORD SHA-1: {0}", hash);
            sb.AppendFormat("{0}\r\n", nextCheckSumLine);
        }

        public string Hash(byte[] temp)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(temp);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
