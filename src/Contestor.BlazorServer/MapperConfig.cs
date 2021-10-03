using AutoMapper;
using Contestor.Proto.Data.Mapper;
using Contestor.Proto.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Contestor.BlazorServer
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
