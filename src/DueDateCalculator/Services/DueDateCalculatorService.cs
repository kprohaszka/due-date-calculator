using DueDateCalculator.Models;

namespace DueDateCalculator.Services
{
    public class DueDateCalculatorService(IWorkingCalendar workingCalendar, WorkingHoursConfiguration config)
        : IDueDateCalculator
    {
        private readonly IWorkingCalendar _workingCalendar = workingCalendar ?? throw new ArgumentNullException(nameof(workingCalendar));
        private readonly WorkingHoursConfiguration _config = config ?? throw new ArgumentNullException(nameof(config));

        public DateTime CalculateDueDate(SubmitInfo submitInfo)
        {
            ValidateInput(submitInfo);
            return ProcessDueDateCalculation(submitInfo.SubmitDate, submitInfo.TurnaroundTimeInHours);
        }

        private void ValidateInput(SubmitInfo submitInfo)
        {
            ArgumentNullException.ThrowIfNull(submitInfo);

            if (submitInfo.TurnaroundTimeInHours < 0)
            {
                throw new ArgumentException("Turnaround time cannot be negative.", nameof(submitInfo.TurnaroundTimeInHours));
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
                if (_workingCalendar.IsWorkingDay(currentDate))
                {
                    var workingHoursInCurrentDay = _workingCalendar.GetRemainingWorkingHoursInDay(currentDate);

                    if (totalWorkingHours + workingHoursInCurrentDay >= turnaroundTimeInHours)
                    {
                        var remainingHours = turnaroundTimeInHours - totalWorkingHours;
                        return CalculateFinalDueDate(currentDate, remainingHours);
                    }

                    totalWorkingHours += workingHoursInCurrentDay;
                }

                currentDate = _workingCalendar.GetNextWorkingDay(currentDate);
            }

            throw new InvalidOperationException("Unable to calculate due date.");
        }

        private DateTime CalculateFinalDueDate(DateTime date, int remainingHours)
        {
            var dueDateTime = _workingCalendar.CreateDateTimeWithHour(date, _config.WorkDayStartHour + remainingHours);

            if (dueDateTime.Hour <= _config.WorkDayEndHour)
            {
                return dueDateTime;
            }

            var hoursIntoNextDay = dueDateTime.Hour - _config.WorkDayEndHour;
            var nextWorkDay = _workingCalendar.GetNextWorkingDay(dueDateTime);

            return _workingCalendar.CreateDateTimeWithHour(nextWorkDay, _config.WorkDayStartHour + hoursIntoNextDay - 1);
        }
    }
}
