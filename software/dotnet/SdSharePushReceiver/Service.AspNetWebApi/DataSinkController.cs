using System;
using System.Threading.Tasks;
using System.Web.Http;
using SdShare.Configuration;

namespace SdShare.Service.AspNetWebApi
{
    public class DataSinkController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post(string resource, string graph)
        {
            try
            {
                var body = await Request.Content.ReadAsStringAsync();
                foreach (var receiver in EndpointConfiguration.GetConfiguredReceivers(graph))
                {
                    receiver.Receive(resource, graph, body);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
