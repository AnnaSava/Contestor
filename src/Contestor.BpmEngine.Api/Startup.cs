using AutoMapper;
using Contestor.BpmEngine.Contract;
using Contestor.BpmEngine.Service;
using Contestor.Data;
using Contestor.Data.Contract.Interfaces;
using Contestor.Data.Mapper;
using Contestor.Data.Services;
using Contestor.Service.Contract;
using Contestor.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contestor.BpmEngine.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ContestDbContext>(options =>
            {
                options
                    .UseNpgsql(Configuration.GetConnectionString("ContestConnection"), b => b.MigrationsAssembly("Contestor.Migrations.PostgreSql"))
                    .UseSnakeCaseNamingConvention();
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ContestMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contestor.BpmEngine.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contestor.BpmEngine.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
