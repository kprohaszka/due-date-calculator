namespace DueDateCalculator.Services
{
    public class WorkingCalendar(WorkingHoursConfiguration config) : IWorkingCalendar
    {
        private readonly WorkingHoursConfiguration _config = config ?? throw new ArgumentNullException(nameof(config));

        public bool IsWorkingDay(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        public int GetRemainingWorkingHoursInDay(DateTime date)
        {
            var workDayStart = CreateDateTimeWithHour(date, _config.WorkDayStartHour);
            var workDayEnd = CreateDateTimeWithHour(date, _config.WorkDayEndHour);

            if (date < workDayStart)
            {
                return _config.WorkingHoursPerDay;
            }

            if (date > workDayEnd)
            {
                return 0;
            }

            return _config.WorkDayEndHour - date.Hour;
        }

        public DateTime GetNextWorkingDay(DateTime date)
        {
            var nextDay = date.AddDays(1);
            return CreateDateTimeWithHour(nextDay, _config.WorkDayStartHour);
        }

        public DateTime CreateDateTimeWithHour(DateTime date, int hour)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, date.Minute, 0, date.Kind);
        }
    }
}