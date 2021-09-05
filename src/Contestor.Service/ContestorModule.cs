using AutoMapper;
using Contestor.BpmEngine.Contract;
using Contestor.BpmEngine.Service;
using Contestor.Data;
using Contestor.Data.Contract;
using Contestor.Data.Contract.Interfaces;
using Contestor.Data.Entities;
using Contestor.Data.Services;
using Contestor.Service.Contract;
using Contestor.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Service
{
    public static class ContestorModule
    {
        public static void AddContestor(this IServiceCollection services, IConfiguration config)
        {
            //services.AddDbContext<ContestDbContext>(options =>
            //{
            //    options
            //        .UseNpgsql(Configuration.GetConnectionString("ContestConnection"), b => b.MigrationsAssembly("Contestor.Migrations.PostgreSql"))
            //        .UseSnakeCaseNamingConvention();
            //});

            services.AddDbContextFactory<ContestDbContext>(options =>
            {
                options
                    .UseNpgsql(config.GetConnectionString("ContestConnection"), b => b.MigrationsAssembly("Contestor.Migrations.PostgreSql"))
                    .UseSnakeCaseNamingConvention();
            });

            services.AddScoped<ContestDbContext>(p =>
                p.GetRequiredService<IDbContextFactory<ContestDbContext>>()
                .CreateDbContext());

            services.AddScoped<IContestDalService>(s => new ContestDalService(
                s.GetService<ContestDbContext>(),
                s.GetService<IMapper>()));

            services.AddHttpClient<IBpmEngineClient, BpmEngineClient>(
               client =>
               {
                   client.BaseAddress = new Uri(config["BpmEngineUri"]);
               });

            services.AddScoped<IContestService>(s => new ContestService(
                s.GetService<IContestDalService>(),
                s.GetService<IBpmEngineClient>(),
                s.GetService<IMapper>()));
        }

        public static void AddUser(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ContestDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserDalService>(s => new UserDalService(
                s.GetService<UserManager<User>>(),
                s.GetService<SignInManager<User>>(),
                s.GetService<IMapper>()));

            services.AddScoped<IUserService>(s => new UserService(
                s.GetService<IUserDalService>(),
                s.GetService<IMapper>()));
        }
    }
}
