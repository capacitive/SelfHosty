using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace SelfHosty
{
    class Program
    {
        static void Main(string[] args)
        {
            const string baseUrl = "http://localhost:9090";

            //Start OWIN host:
            using (WebApp.Start<Startup>(url: baseUrl))
            {
                Console.WriteLine("Web API listening at: " + baseUrl);
                Console.ReadKey();
            }
        }
    }
}
