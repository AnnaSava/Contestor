using AutoMapper;
using Contestor.BlazorServer.Data;
using Contestor.BpmEngine.Contract;
using Contestor.BpmEngine.Service;
using Contestor.Data;
using Contestor.Data.Contract;
using Contestor.Data.Contract.Interfaces;
using Contestor.Data.Entities;
using Contestor.Data.Mapper;
using Contestor.Data.Services;
using Contestor.Service.Contract;
using Contestor.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contestor.BlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ContestDbContext>(options =>
            {
                options
                    .UseNpgsql(Configuration.GetConnectionString("ContestConnection"), b => b.MigrationsAssembly("Contestor.Migrations.PostgreSql"))
                    .UseSnakeCaseNamingConvention();
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ContestDbContext>()
                .AddDefaultTokenProviders();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ContestMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            services.AddScoped<IContestDalService>(s => new ContestDalService(
                s.GetService<ContestDbContext>(),
                s.GetService<IMapper>()));

            services.AddHttpClient<IBpmEngineClient, BpmEngineClient>(
               client =>
               {
                   client.BaseAddress = new Uri(Configuration["BpmEngineUri"]);
               });

            services.AddScoped<IContestService>(s => new ContestService(
                s.GetService<IContestDalService>(),
                s.GetService<IBpmEngineClient>()));

            services.AddScoped<IUserDalService>(s => new UserDalService(
                s.GetService<UserManager<User>>(),
                s.GetService<SignInManager<User>>(),
                s.GetService<IMapper>()));

            services.AddScoped<IUserService>(s => new UserService(
                s.GetService<IUserDalService>(),
                s.GetService<IMapper>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
