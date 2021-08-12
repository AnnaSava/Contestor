using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Если падает ошибка System.Net.Sockets.SocketException, зайти в cmd, ipconfig /all
            // посмотреть IPv4-адрес у Hyper-V Virtual Ethernet Adapter и вбить его в appsettings и launchsettings
            // И не забыть поменять этот адрес в БП

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
