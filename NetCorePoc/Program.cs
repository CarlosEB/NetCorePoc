using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;

namespace NetCorePoc.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHostKestrel(args).Run();
        }

        public static IWebHost BuildWebHostKestrel(string[] args) => WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseKestrel()
            .UseUrls("http://*:5001")
            .Build();

        public static IWebHost BuildWebHostHttpSys(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseHttpSys(options =>
                {
                    options.Authentication.Schemes = AuthenticationSchemes.None;
                    options.Authentication.AllowAnonymous = true;
                    options.MaxConnections = 100;
                    options.MaxRequestBodySize = 30000000;
                    options.UrlPrefixes.Add("http://localhost:5001");
                })
                .Build();
    }
}
