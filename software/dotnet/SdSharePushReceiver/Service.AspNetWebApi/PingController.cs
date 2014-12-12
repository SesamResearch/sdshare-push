using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SdShare.Configuration;
using SdShare.Documentation;

namespace SdShare.Service.AspNetWebApi
{
    public class PingController : ApiController
    {
        [HttpGet]
        public string Ping()
        {
            return "Ping received. All is well.";
        }

        [HttpGet]
        public StatusDocumentation Doc()
        {
            return EndpointConfiguration.GetDocumentation();
        }

        [HttpGet]
        public string Sample(string type)
        {
            var transformDoc = EndpointConfiguration.GetDocumentation().Transforms.SingleOrDefault(t => t.RdfType == type);
            if (transformDoc == null)
            {
                return null;
            }

            return transformDoc.GetSampleNTriples();
        }

        [HttpGet]
        public IEnumerable<ExceptionLogInfo> Exceptions(string after = null)
        {
            return ExceptionLogs.GetExceptions(GetDateTime(after));
        }

        private DateTime GetDateTime(string after)
        {
            return string.IsNullOrWhiteSpace(after)
                ? DateTime.MinValue
                : DateTime.Parse(after);
        }
    }
}
