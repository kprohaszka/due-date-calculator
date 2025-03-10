using DueDateCalculator.Models;
using DueDateCalculator.Services;

namespace DueDateCalculator.Tests;

public class DueDateCalculatorTests
{
    [Fact]
    public void CalculateDueDate_TuesdaySubmit_ThursdayDue()
    {
        var submitInfo = new SubmitInfo
        {
            SubmitDate = new DateTime(2023, 1, 3, 14, 12, 0),
            TurnaroundTimeInHours = 16
        };

        var calculator = new DueDateCalculatorService();
        
        var dueDate = calculator.CalculateDueDate(submitInfo);
        
        Assert.Equal(new DateTime(2023, 1, 5, 14, 12, 0), dueDate);
    }
}