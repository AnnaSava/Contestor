using Contestor.Proto;
using Contestor.Proto.Data.Entities;
using Contestor.Proto.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Contestor.Proto.Seeder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Contestor Data seeder started!");

            IConfigurationRoot config = GetConfiguration(GetEnvironment());
            var services = new ServiceCollection();
            services.AddLogging(); // TODO: без него не работает идентити. разобраться

            services.AddMapper();
            services.AddUser(config);
            services.AddContestor(config);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                using (IServiceScope scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    await Seed(scope);
                }
            }

            Console.WriteLine("Contestor Data seeder finished!");
        }

        private static string GetEnvironment()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return string.IsNullOrEmpty(env) ? "Production" : env;
        }

        private static IConfigurationRoot GetConfiguration(string environmentName)
        {
            string appsettingsPath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "appsettings.json"));

            string appsettingsEnvPath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, $"appsettings.{environmentName}.json"));

            return new ConfigurationBuilder()
                .AddJsonFile(appsettingsPath, true, true)
                .AddJsonFile(appsettingsEnvPath, true, true)
                .Build();
        }

        private static async Task Seed(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetService<ContestDbContext>();
            context.Database.Migrate();

            var adminExists = context.Users.Any(m => m.UserName == "admin");

            if (!adminExists)
            {
                var mgr = scope.ServiceProvider.GetService<UserManager<User>>();

                var res = await mgr.CreateAsync(new User() { Email = "test@test.ru", UserName = "admin" }, "Pass123$");
            }
        }
    }
}
