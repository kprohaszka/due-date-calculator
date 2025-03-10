using DueDateCalculator.Models;

namespace DueDateCalculator.Services;

public class DueDateCalculatorService
{
    private const int WorkingHoursPerDay = 8;
    private const int StartHour = 9;
    private const int EndHour = 17;

    public DateTime CalculateDueDate(SubmitInfo submitInfo)
    {
        if (submitInfo.TurnaroundTimeInHours < 0)
        {
            throw new ArgumentException("Turnaround time cannot be negative.",
                nameof(submitInfo.TurnaroundTimeInHours));
        }

        return CalculateDueDateInternal(submitInfo.SubmitDate, submitInfo.TurnaroundTimeInHours);
    }

    private DateTime CalculateDueDateInternal(DateTime submitDate, int turnaroundTimeInHours)
    {
        if (turnaroundTimeInHours == 0)
        {
            return submitDate;
        }

        var totalWorkingHours = 0;
        var currentDate = submitDate;

        while (totalWorkingHours < turnaroundTimeInHours)
        {
            if (IsWorkingDay(currentDate))
            {
                var hoursInCurrentDay = GetWorkingHoursInDay(currentDate);

                if (totalWorkingHours + hoursInCurrentDay >= turnaroundTimeInHours)
                {
                    var remainingHours = turnaroundTimeInHours - totalWorkingHours;
                    var dueDate = AdjustDueDate(currentDate, remainingHours);

                    return dueDate;
                }
                else
                {
                    totalWorkingHours += hoursInCurrentDay;
                    currentDate = MoveToNextWorkingDay(currentDate);
                }
            }
            else
            {
                currentDate = MoveToNextWorkingDay(currentDate);
            }
        }

        throw new InvalidOperationException("Unable to calculate due date.");
    }

    private bool IsWorkingDay(DateTime date)
    {
        return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
    }

    private int GetWorkingHoursInDay(DateTime date)
    {
        var startOfWorkingDay = new DateTime(date.Year, date.Month, date.Day, StartHour, 0, 0);
        var endOfWorkingDay = new DateTime(date.Year, date.Month, date.Day, EndHour, 0, 0);

        if (date < startOfWorkingDay)
        {
            return EndHour - StartHour;
        }
        else if (date > endOfWorkingDay)
        {
            return 0;
        }
        else
        {
            return EndHour - date.Hour;
        }
    }

    private DateTime MoveToNextWorkingDay(DateTime date)
    {
        date = date.AddDays(1);
        return new DateTime(date.Year, date.Month, date.Day, StartHour, date.Minute, 0);
    }

    private DateTime AdjustDueDate(DateTime date, int remainingHours)
    {
        var dueDate = new DateTime(date.Year, date.Month, date.Day, StartHour + remainingHours, date.Minute, 0);

        if (dueDate.Hour <= EndHour) return dueDate;
        var extraHours = dueDate.Hour - EndHour;
        dueDate = dueDate.AddDays(1).AddHours(-extraHours);
        dueDate = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, StartHour, date.Minute, 0);

        return dueDate;
    }
}