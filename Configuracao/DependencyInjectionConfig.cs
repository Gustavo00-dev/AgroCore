using APIAgroCoreOrquestradora.Service;

namespace APIAgroCoreOrquestradora.Configuracao
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            #region Services/Repository   
            services.AddScoped<ILoginService, LoginService>();
            #endregion

            return services;
        }
    }
}
