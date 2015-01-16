using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SdShare.Documentation
{
    public class ExceptionLogParser
    {
        private const string LogFile = @".\exceptions.log";

        private const string RegexLinePrefix = @"20\d{2}-[0,1]\d-[0-3]\d \d{2}:\d{2}:\d{2}.\d{4} SdShare.PushReceiver.Exceptions";
        private const string RegexDatePrefix = @"20\d{2}-[0,1]\d-[0-3]\d \d{2}:\d{2}:\d{2}.\d{4}";

        private readonly Regex _regexLinePrefix = new Regex(RegexLinePrefix);
        private readonly Regex _regexDatePrefix = new Regex(RegexDatePrefix);

        public IEnumerable<ExceptionLogInfo> Parse()
        {
            if (!File.Exists(LogFile))
            {
                return new List<ExceptionLogInfo>();
            }

            return Parse(File.ReadAllLines(LogFile));
        }

        public IEnumerable<ExceptionLogInfo> Parse(IList<string> lines)
        {
            var result = new List<ExceptionLogInfo>();

            ExceptionLogInfo current = null;
            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines[i];
                if (line.Contains("STARTEXCEPTION"))
                {
                    var m = _regexDatePrefix.Match(line);
                    current = new ExceptionLogInfo {TimeUtc = DateTime.Parse(line.Substring(m.Index, m.Length))};
                    result.Add(current);
                    continue;
                }

                if (line.Contains("STARTRESOURCES"))
                {
                    i = ParseResources(i, current, lines);
                    continue;
                }

                if (line.Contains("STARTPAYLOAD"))
                {
                    i = ParsePayload(i, current, lines);
                    continue;
                }

                if (line.Contains("STARTTARGETPAYLOAD"))
                {
                    i = ParseTargetPayload(i, current, lines);
                    continue;
                }

                if (line.Contains("STARTDETAILS"))
                {
                    i = ParseExceptionDetails(i, current, lines);
                }
            }

            return result;
        }

        private int ParseExceptionDetails(int i, ExceptionLogInfo current, IList<string> lines)
        {
            i++;
            var done = false;
            var sb = new StringBuilder();
            do
            {
                var line = lines[i];
                done = line.Contains("ENDDETAILS");
                if (!done)
                {
                    var wl = WashLine(line);
                    if (wl != null)
                    {
                        sb.AppendLine(wl);
                    }
                }

                i++;
            } while (!done);

            current.ExceptionDetails = sb.ToString();
            return i;
        }

        private int ParsePayload(int i, ExceptionLogInfo current, IList<string> lines)
        {
            i++;
            var done = false;
            var sb = new StringBuilder();
            do
            {
                var line = lines[i];
                done = line.Contains("ENDPAYLOAD");
                if (!done)
                {
                    var wl = WashLine(line);
                    if (wl != null)
                    {
                        sb.AppendLine(wl);
                    }
                }

                i++;
            } while (!done);

            current.Payload = sb.ToString();
            return i;
        }

        private int ParseTargetPayload(int i, ExceptionLogInfo current, IList<string> lines)
        {
            i++;
            var done = false;
            var sb = new StringBuilder();
            do
            {
                var line = lines[i];
                done = line.Contains("ENDTARGETPAYLOAD");
                if (!done)
                {
                    var wl = WashLine(line);
                    if (wl != null)
                    {
                        sb.AppendLine(wl);
                    }
                }

                i++;
            } while (!done);

            current.TargetPayload = sb.ToString();
            return i;
        }

        private int ParseResources(int i, ExceptionLogInfo current, IList<string> lines)
        {
            i++;
            var done = false;
            var resources = new List<string>();
            current.Resources = resources;
            do
            {
                var line = lines[i];
                done = line.Contains("ENDRESOURCES");
                if (!done)
                {
                    var wl = WashLine(line);
                    if (wl != null)
                    {
                        resources.Add(wl);
                    }
                }
                
                i++;
            } while (!done);

            return i;
        }

        private string WashLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }

            var m = _regexLinePrefix.Match(line);
            if (!m.Success)
            {
                return line.Trim();
            }

            var start = m.Index + m.Length;
            return line.Substring(start, line.Length - start).Trim();
        }
    }
}
