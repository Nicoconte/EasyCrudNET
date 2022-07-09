using Microsoft.Extensions.DependencyInjection;

namespace EasyCrudNET.Configurations
{
    
    public static class EasyCrudConfiguration
    {
        public static IServiceCollection AddEasyCrud(this IServiceCollection services, Action<EasyCrudConfigurationOptions> setupOptions)
        {
            var options = new EasyCrudConfigurationOptions();

            //Call action and setup options 
            setupOptions.Invoke(options);

            EasyCrud easyCrud = new EasyCrud();
            easyCrud.SetSqlConnection(options.ConnectionString);

            services.AddSingleton(easyCrud);

            return services;
        }
    }
}
