using DueDateCalculator.Models;
using DueDateCalculator.Services;
using Moq;

namespace DueDateCalculator.Tests
{
    public class DueDateCalculatorTests
    {
        private readonly IDueDateCalculator _calculator;

        public DueDateCalculatorTests()
        {
            var config = new WorkingHoursConfiguration
            {
                WorkDayStartHour = 9,
                WorkDayEndHour = 17,
                WorkingHoursPerDay = 8
            };

            var workingCalendar = new WorkingCalendar(config);
            _calculator = new DueDateCalculatorService(workingCalendar, config);
        }

        [Fact]
        public void CalculateDueDate_MondaySubmit_WednesdayDue()
        {
            var submitInfo = new SubmitInfo
            {
                SubmitDate = new DateTime(2023, 1, 2, 14, 12, 0),
                TurnaroundTimeInHours = 16
            };

            var dueDate = _calculator.CalculateDueDate(submitInfo);

            Assert.Equal(new DateTime(2023, 1, 4, 14, 12, 0), dueDate);
        }

        [Fact]
        public void CalculateDueDate_FridaySubmit_TuesdayDue()
        {
            var submitInfo = new SubmitInfo
            {
                SubmitDate = new DateTime(2023, 1, 6, 14, 12, 0),
                TurnaroundTimeInHours = 16
            };

            var dueDate = _calculator.CalculateDueDate(submitInfo);

            Assert.Equal(new DateTime(2023, 1, 10, 14, 12, 0), dueDate);
        }

        [Fact]
        public void CalculateDueDate_SubmissionAt9AM()
        {
            var submitInfo = new SubmitInfo
            {
                SubmitDate = new DateTime(2023, 1, 3, 9, 0, 0),
                TurnaroundTimeInHours = 16
            };

            var dueDate = _calculator.CalculateDueDate(submitInfo);

            Assert.Equal(new DateTime(2023, 1, 4, 17, 0, 0), dueDate);
        }

        [Fact]
        public void CalculateDueDate_ZeroTurnaroundTime()
        {
            var submitInfo = new SubmitInfo
            {
                SubmitDate = new DateTime(2023, 1, 3, 14, 12, 0),
                TurnaroundTimeInHours = 0
            };

            var dueDate = _calculator.CalculateDueDate(submitInfo);

            Assert.Equal(new DateTime(2023, 1, 3, 14, 12, 0), dueDate);
        }

        [Fact]
        public void CalculateDueDate_NegativeTurnaroundTime_ThrowsException()
        {
            var submitInfo = new SubmitInfo
            {
                SubmitDate = new DateTime(2023, 1, 3, 14, 12, 0),
                TurnaroundTimeInHours = -1
            };

            Assert.Throws<ArgumentException>(() => _calculator.CalculateDueDate(submitInfo));
        }

        [Fact]
        public void CalculateDueDate_NullSubmitInfo_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _calculator.CalculateDueDate(null));
        }
    }

    public class WorkingCalendarTests
    {
        private readonly WorkingCalendar _workingCalendar;

        public WorkingCalendarTests()
        {
            var config = new WorkingHoursConfiguration
            {
                WorkDayStartHour = 9,
                WorkDayEndHour = 17,
                WorkingHoursPerDay = 8
            };

            _workingCalendar = new WorkingCalendar(config);
        }

        [Theory]
        [InlineData(2023, 1, 2)]
        [InlineData(2023, 1, 3)]
        [InlineData(2023, 1, 4)]
        [InlineData(2023, 1, 5)]
        [InlineData(2023, 1, 6)]
        public void IsWorkingDay_Weekday_ReturnsTrue(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);

            var result = _workingCalendar.IsWorkingDay(date);

            Assert.True(result);
        }

        [Theory]
        [InlineData(2023, 1, 7)] // Saturday
        [InlineData(2023, 1, 8)] // Sunday
        public void IsWorkingDay_Weekend_ReturnsFalse(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);

            var result = _workingCalendar.IsWorkingDay(date);

            Assert.False(result);
        }

        [Fact]
        public void GetRemainingWorkingHoursInDay_BeforeWorkingHours_ReturnsFullDay()
        {
            var date = new DateTime(2023, 1, 2, 8, 0, 0);

            var hours = _workingCalendar.GetRemainingWorkingHoursInDay(date);

            Assert.Equal(8, hours);
        }

        [Fact]
        public void GetRemainingWorkingHoursInDay_DuringWorkingHours_ReturnsRemainingHours()
        {
            var date = new DateTime(2023, 1, 2, 13, 0, 0);

            var hours = _workingCalendar.GetRemainingWorkingHoursInDay(date);

            Assert.Equal(4, hours);
        }

        [Fact]
        public void GetRemainingWorkingHoursInDay_AfterWorkingHours_ReturnsZero()
        {
            var date = new DateTime(2023, 1, 2, 18, 0, 0);

            var hours = _workingCalendar.GetRemainingWorkingHoursInDay(date);

            Assert.Equal(0, hours);
        }
    }

    public class DueDateCalculatorWithMockTests
    {
        [Fact]
        public void CalculateDueDate_UsesWorkingCalendarCorrectly()
        {
            var mockWorkingCalendar = new Mock<IWorkingCalendar>();
            var config = new WorkingHoursConfiguration
            {
                WorkDayStartHour = 9,
                WorkDayEndHour = 17
            };

            var submitDate = new DateTime(2023, 1, 2, 14, 0, 0);
            var nextWorkingDay = new DateTime(2023, 1, 3);
            var expectedDueDate = new DateTime(2023, 1, 3, 10, 0, 0);

            var submitInfo = new SubmitInfo
            {
                SubmitDate = submitDate,
                TurnaroundTimeInHours = 4
            };

            mockWorkingCalendar.Setup(x => x.IsWorkingDay(submitDate)).Returns(true);
            mockWorkingCalendar.Setup(x => x.IsWorkingDay(nextWorkingDay)).Returns(true);

            mockWorkingCalendar.Setup(x => x.GetRemainingWorkingHoursInDay(submitDate)).Returns(3);
            mockWorkingCalendar.Setup(x => x.GetRemainingWorkingHoursInDay(nextWorkingDay)).Returns(8);

            mockWorkingCalendar.Setup(x => x.GetNextWorkingDay(submitDate)).Returns(nextWorkingDay);

            mockWorkingCalendar.Setup(x => x.CreateDateTimeWithHour(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns((DateTime date, int hour) => new DateTime(date.Year, date.Month, date.Day, hour, 0, 0));

            var calculator = new DueDateCalculatorService(mockWorkingCalendar.Object, config);

            var result = calculator.CalculateDueDate(submitInfo);

            mockWorkingCalendar.Verify(x => x.IsWorkingDay(It.IsAny<DateTime>()), Times.AtLeastOnce);
            mockWorkingCalendar.Verify(x => x.GetRemainingWorkingHoursInDay(It.IsAny<DateTime>()), Times.AtLeastOnce);
            mockWorkingCalendar.Verify(x => x.GetNextWorkingDay(It.IsAny<DateTime>()), Times.AtLeastOnce);
            
            Assert.Equal(expectedDueDate, result);
        }
    }
}