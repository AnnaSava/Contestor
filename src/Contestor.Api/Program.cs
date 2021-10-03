using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Contestor.Proto.Api
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
