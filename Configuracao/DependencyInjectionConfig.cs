using APIAgroCoreOrquestradora.Service;
using Microsoft.Extensions.DependencyInjection;

namespace APIAgroCoreOrquestradora.Configuracao
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            #region Services/Repository   
            services.AddHttpClient<IUserService, UserService>();
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            services.AddScoped<IDadosService, DadosService>();
            #endregion

            return services;
        }
    }
}
