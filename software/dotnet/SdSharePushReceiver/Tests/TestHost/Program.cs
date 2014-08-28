using System;
using Microsoft.Owin.Hosting;
using SdShare.Configuration;
using Service.AspNetWebApi;

namespace TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = EndpointConfiguration.BaseAddress;

            // Start OWIN host 
            using (WebApp.Start<SetUpConfiguration>(url: baseAddress))
            {
                Console.WriteLine(EndpointConfiguration.GetReport());
                Console.ReadLine(); 
            }


        }
    }
}
