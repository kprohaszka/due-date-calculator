namespace DueDateCalculator.Services
{
    public interface IWorkingCalendar
    {
        bool IsWorkingDay(DateTime date);
        
        int GetRemainingWorkingHoursInDay(DateTime date);
        
        DateTime GetNextWorkingDay(DateTime date);
        
        DateTime CreateDateTimeWithHour(DateTime date, int hour);
    }
}