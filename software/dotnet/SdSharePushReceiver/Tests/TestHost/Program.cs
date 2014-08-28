using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using SdShare.Configuration;
using Service.AspNetWebApi;

namespace TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = EndpointConfiguration.BaseAddress;

            // Start OWIN host 
            using (WebApp.Start<SetUpConfiguration>(url: baseAddress))
            {
                Console.WriteLine(EndpointConfiguration.GetReport());
                Console.ReadLine(); 
            }


        }
    }
}
