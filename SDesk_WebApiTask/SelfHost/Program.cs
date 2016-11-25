using System;
using System.Web.Http.SelfHost;
using SDSK.API;

namespace SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:9000");
            
            WebApiConfig.Register(config);
            
            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Local host is runnig.\nPress Enter to stop it and quit.");
                Console.ReadLine();
            }
        }
    }
}
