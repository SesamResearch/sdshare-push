using System;
using System.Linq;
using System.Net;
using Microsoft.Owin.Hosting;
using NetTriple;
using SdShare.Configuration;
using SdShare.Metadata;
using SdShare.Service.AspNetWebApi;

namespace ServiceRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadAllRdfClasses.LoadTransforms(MetaProvider.GetTransforms().ToArray());

            var port = EndpointConfiguration.Port;

            var options = new StartOptions();
            options.Urls.Add(string.Format("http://localhost:{0}/", port));
            options.Urls.Add(string.Format("http://127.0.0.1:{0}/", port));
            var entry = Dns.GetHostEntry(Dns.GetHostName());
            options.Urls.Add(string.Format("http://{0}:{1}/", entry.HostName, port));
            
            // Start OWIN host 
            using (WebApp.Start<SetUpConfiguration>(options))
            {
                Console.WriteLine(EndpointConfiguration.GetReport());
                Console.ReadLine(); 
            }
        }
    }
}
