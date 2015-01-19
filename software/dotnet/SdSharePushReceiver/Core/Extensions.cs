using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace SdShare
{
    public static class Extensions
    {
        public static string ToConcatenatedString(this IEnumerable<string> strs)
        {
            if (strs == null || !strs.Any())
            {
                return null;
            }

            var started = false;
            return strs.Aggregate(new StringBuilder(), (sb, r) =>
            {
                if (started)
                {
                    sb.Append(";");
                }

                started = true;
                sb.Append(r);
                return sb;
            }).ToString();
        }
    }
}
