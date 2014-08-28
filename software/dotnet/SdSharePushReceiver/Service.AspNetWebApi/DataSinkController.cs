using System.Web.Http;
using SdShare.Configuration;

namespace Service.AspNetWebApi
{
    public class DataSinkController : ApiController
    {
        [HttpPost]
        public async void Post(string resource, string graph)
        {
            var body = await Request.Content.ReadAsStringAsync();
            foreach (var receiver in ReceiverManager.GetReceivers(graph))
            {
                receiver.Receive(resource, graph, body);
            }
        }
    }
}
