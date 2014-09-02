using System.Web.Http;
using SdShare.Configuration;

namespace SdShare.Service.AspNetWebApi
{
    public class DataSinkController : ApiController
    {
        [HttpPost]
        public async void Post(string resource, string graph)
        {
            var body = await Request.Content.ReadAsStringAsync();
            var triples = body.ToWashedTriplePayload();
            foreach (var receiver in EndpointConfiguration.GetConfiguredReceivers(graph))
            {
                receiver.Receive(resource, graph, triples);
            }
        }
    }
}
