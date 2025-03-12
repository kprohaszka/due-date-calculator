using Microsoft.Extensions.DependencyInjection;

namespace DueDateCalculator.Services
{
    public static class DueDateCalculatorServiceExtensions
    {
        public static IServiceCollection AddDueDateCalculator(this IServiceCollection services, Action<WorkingHoursConfiguration> configAction = null)
        {
            services.AddSingleton<WorkingHoursConfiguration>(serviceProvider => 
            {
                var config = new WorkingHoursConfiguration();
                configAction?.Invoke(config);
                return config;
            });

            services.AddSingleton<WorkingCalendar>();

            services.AddScoped<IDueDateCalculator, DueDateCalculatorService>();

            return services;
        }
    }
}