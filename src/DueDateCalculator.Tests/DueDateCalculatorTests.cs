using DueDateCalculator.Models;
using DueDateCalculator.Services;

namespace DueDateCalculator.Tests;

public class DueDateCalculatorTests
{
    [Fact]
    public void CalculateDueDate_MondaySubmit_WednesdayDue()
    {
        var submitInfo = new SubmitInfo
        {
            SubmitDate = new DateTime(2023, 1, 2, 14, 12, 0),
            TurnaroundTimeInHours = 16
        };

        var calculator = new DueDateCalculatorService();

        var dueDate = calculator.CalculateDueDate(submitInfo);

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

        var calculator = new DueDateCalculatorService();

        var dueDate = calculator.CalculateDueDate(submitInfo);

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

        var calculator = new DueDateCalculatorService();

        var dueDate = calculator.CalculateDueDate(submitInfo);

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

        var calculator = new DueDateCalculatorService();

        var dueDate = calculator.CalculateDueDate(submitInfo);

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

        var calculator = new DueDateCalculatorService();

        Assert.Throws<ArgumentException>(() => calculator.CalculateDueDate(submitInfo));
    }
}