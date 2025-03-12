using DueDateCalculator.Models;
using DueDateCalculator.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DueDateCalculator;

class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<WorkingHoursConfiguration>();
        services.AddSingleton<WorkingCalendar>();
        services.AddScoped<IDueDateCalculator, DueDateCalculatorService>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        var calculator = serviceProvider.GetRequiredService<IDueDateCalculator>();
        
        var result = calculator.CalculateDueDate(new SubmitInfo 
        { 
            SubmitDate = DateTime.Now, 
            TurnaroundTimeInHours = 16 
        });
    }
}