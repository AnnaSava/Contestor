﻿using AutoMapper;
using Contestor.Data.Mapper;
using Contestor.Service.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contestor.Seeder
{
    public static class MapperConfig
    {
        public static void AddMapper(this IServiceCollection services)
        {
            // Auto Mapper Configurations
            // Можно добавлять все профили из сборки, но в нашем случае всё равно прописывать каждую сборку - надо разбираться
            // https://stackoverflow.com/questions/2651613/how-to-scan-and-auto-configure-profiles-in-automapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ContestDataMapperProfile());
                mc.AddProfile(new ContestMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
