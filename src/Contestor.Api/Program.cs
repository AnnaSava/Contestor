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
            // ���� ������ ������ System.Net.Sockets.SocketException, ����� � cmd, ipconfig /all
            // ���������� IPv4-����� � Hyper-V Virtual Ethernet Adapter � ����� ��� � appsettings � launchsettings
            // � �� ������ �������� ���� ����� � ��

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
