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
    }
}
