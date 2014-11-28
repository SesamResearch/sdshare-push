using System.Web.Http;
using System.Web.Http.Dispatcher;
using Owin;

namespace SdShare.Service.AspNetWebApi
{
    public class SetUpConfiguration
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "",
                new { controller = "DataSink", action = "Post" });

            config.Routes.MapHttpRoute(
                "PingRoute",
                "ping",
                new { controller = "Ping", action = "Ping" });

            config.Routes.MapHttpRoute(
                "DocRoute",
                "doc",
                new { controller = "Ping", action = "Doc" });

            config.Routes.MapHttpRoute(
                "SampleTriplesRoute",
                "sample",
                new { controller = "Ping", action = "Sample" });

            config.Services.Replace(typeof(IAssembliesResolver), new SdShareWebApiAssemblyResolver());

            appBuilder.UseWebApi(config);
        }
    } 
}
