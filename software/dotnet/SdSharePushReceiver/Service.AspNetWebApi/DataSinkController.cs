using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using SdShare.Configuration;
using SdShare.Diagnostics;
using SdShare.Exceptions;

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

                var receivers = EndpointConfiguration.GetConfiguredReceivers(graph);
                if (receivers == null || !receivers.Any())
                {
                    throw new MissingGraphException(string.Format("No receivers configured for graph {0}", graph));
                }

                foreach (var receiver in receivers)
                {
                    receiver.Receive(resource, graph, body);
                }

                DiagnosticData.IncResources(resource.Count);
                return Ok();
            }
            catch (MissingGraphException mge)
            {
                ExceptionWriter.Write(mge, resource, null);

                DiagnosticData.IncErrors();
                return InternalServerError(mge);
            }
            catch (Exception e)
            {
                DiagnosticData.IncErrors();
                return InternalServerError(e);
            }
        }
    }
}
