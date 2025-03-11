using DueDateCalculator.Models;

namespace DueDateCalculator.Services;

public class DueDateCalculatorService
{
    private const int WorkingHoursPerDay = 8;
    private const int WorkDayStartHour = 9;
    private const int WorkDayEndHour = 17;

    public DateTime CalculateDueDate(SubmitInfo submitInfo)
    {
        ValidateTurnaroundTime(submitInfo.TurnaroundTimeInHours);
        return ProcessDueDateCalculation(submitInfo.SubmitDate, submitInfo.TurnaroundTimeInHours);
    }

    private void ValidateTurnaroundTime(int turnaroundTimeInHours)
    {
        if (turnaroundTimeInHours < 0)
        {
            throw new ArgumentException("Turnaround time cannot be negative.", nameof(turnaroundTimeInHours));
        }
    }

    private DateTime ProcessDueDateCalculation(DateTime submitDate, int turnaroundTimeInHours)
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
                var workingHoursInCurrentDay = GetRemainingWorkingHoursInDay(currentDate);

                if (totalWorkingHours + workingHoursInCurrentDay >= turnaroundTimeInHours)
                {
                    var remainingHours = turnaroundTimeInHours - totalWorkingHours;
                    return CalculateFinalDueDate(currentDate, remainingHours);
                }

                totalWorkingHours += workingHoursInCurrentDay;
                currentDate = GetNextWorkingDay(currentDate);
            }
            else
            {
                currentDate = GetNextWorkingDay(currentDate);
            }
        }

        throw new InvalidOperationException("Unable to calculate due date.");
    }

    private bool IsWorkingDay(DateTime date)
    {
        return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
    }

    private int GetRemainingWorkingHoursInDay(DateTime date)
    {
        var workDayStart = CreateDateTimeWithHour(date, WorkDayStartHour);
        var workDayEnd = CreateDateTimeWithHour(date, WorkDayEndHour);

        if (date < workDayStart)
        {
            return WorkingHoursPerDay;
        }

        if (date > workDayEnd)
        {
            return 0;
        }

        return WorkDayEndHour - date.Hour;
    }

    private DateTime GetNextWorkingDay(DateTime date)
    {
        var nextDay = date.AddDays(1);
        return CreateDateTimeWithHour(nextDay, WorkDayStartHour);
    }

    private DateTime CalculateFinalDueDate(DateTime date, int remainingHours)
    {
        var dueDateTime = CreateDateTimeWithHour(date, WorkDayStartHour + remainingHours);

        if (dueDateTime.Hour <= WorkDayEndHour)
        {
            return dueDateTime;
        }

        var hoursIntoNextDay = dueDateTime.Hour - WorkDayEndHour;
        var nextWorkDay = GetNextWorkingDay(dueDateTime);

        return CreateDateTimeWithHour(nextWorkDay, WorkDayStartHour + hoursIntoNextDay - 1);
    }

    private DateTime CreateDateTimeWithHour(DateTime date, int hour)
    {
        return new DateTime(date.Year, date.Month, date.Day, hour, date.Minute, 0);
    }
}