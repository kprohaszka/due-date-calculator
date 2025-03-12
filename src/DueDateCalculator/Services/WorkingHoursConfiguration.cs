namespace DueDateCalculator.Services
{
    public class WorkingHoursConfiguration
    {
        public int WorkingHoursPerDay { get; set; } = 8;
        public int WorkDayStartHour { get; set; } = 9;
        public int WorkDayEndHour { get; set; } = 17;
    }
}