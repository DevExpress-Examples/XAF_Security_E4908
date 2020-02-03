using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ASPNETCoreODataService
{
    public class Program
    {
        public static void Main(string[] args)
        {
			DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
			DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
			BuildWebHost(args).Run();
		}

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
