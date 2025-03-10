using DueDateCalculator.Models;

namespace DueDateCalculator.Services;

public class DueDateCalculatorService
{
    public DateTime CalculateDueDate(SubmitInfo submitInfo)
    {
        var submitDate = submitInfo.SubmitDate;
        var turnaroundTimeInHours = submitInfo.TurnaroundTimeInHours;

        var dueDate = CalculateDueDateInternal(submitDate, turnaroundTimeInHours);

        return dueDate;
    }

    private DateTime CalculateDueDateInternal(DateTime submitDate, int turnaroundTimeInHours)
    {
        const int workingHoursPerDay = 8;
        var totalWorkingHours = 0;

        var currentDate = submitDate;

        while (totalWorkingHours < turnaroundTimeInHours)
        {
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                var hoursInCurrentDay = workingHoursPerDay - (currentDate.Hour - 9);

                if (totalWorkingHours + hoursInCurrentDay >= turnaroundTimeInHours)
                {
                    var remainingHours = turnaroundTimeInHours - totalWorkingHours;
                    var minutes = (remainingHours % 1) * 60;
                    var dueDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 9 + remainingHours, minutes, 0);

                    if (dueDate.Hour <= 17) return dueDate;
                    
                    var extraHours = dueDate.Hour - 17;
                    dueDate = dueDate.AddDays(1).AddHours(-extraHours);

                    return dueDate;
                }

                totalWorkingHours += hoursInCurrentDay;
                currentDate = currentDate.AddDays(1).AddHours(-workingHoursPerDay);
            }
            else
            {
                currentDate = currentDate.AddDays(1);
            }
        }

        throw new InvalidOperationException("Unable to calculate due date.");
    }
}
