using AutoMapper;
using Contestor.Proto.BpmEngine;
using Contestor.Proto.BpmEngine.Services;
using Contestor.Proto.Data;
using Contestor.Proto.Data.Entities;
using Contestor.Proto.Data.Services;
using Contestor.Proto.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Contestor.Proto
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
