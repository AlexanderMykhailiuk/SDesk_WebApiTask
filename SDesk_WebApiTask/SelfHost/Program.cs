using System;
using System.Web.Http.SelfHost;
using SDSK.API;

namespace SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080");
            
            WebApiConfig.Register(config);
            
            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                try
                {
                    server.OpenAsync().Wait();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
