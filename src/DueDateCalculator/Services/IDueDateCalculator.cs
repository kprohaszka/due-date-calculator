using DueDateCalculator.Models;

namespace DueDateCalculator.Services
{
    public interface IDueDateCalculator
    {
        DateTime CalculateDueDate(SubmitInfo submitInfo);
    }
}