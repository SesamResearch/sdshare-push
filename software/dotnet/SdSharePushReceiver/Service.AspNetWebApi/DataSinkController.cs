using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using SdShare.Configuration;
using SdShare.Diagnostics;

namespace SdShare.Service.AspNetWebApi
{
    public class DataSinkController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post([ModelBinder]List<string> resource, string graph)
        {
            try
            {
                DiagnosticData.IncRequests();

                var body = await Request.Content.ReadAsStringAsync();
                foreach (var receiver in EndpointConfiguration.GetConfiguredReceivers(graph))
                {
                    receiver.Receive(resource, graph, body);
                }

                DiagnosticData.IncResources(resource.Count);
                return Ok();
            }
            catch (Exception e)
            {
                DiagnosticData.IncErrors();
                return InternalServerError(e);
            }
        }
    }
}
