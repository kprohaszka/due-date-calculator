# Due Date Calculator

Welcome to the **Due Date Calculator** project! This repository contains a solution to calculate the due date for issues reported in an issue-tracking system, adhering to specific rules regarding working hours and turnaround time. The implementation is designed to be clean, professional, and production-ready, with automated tests included for validation.

## Overview

The **Due Date Calculator** is a software solution designed to determine the resolution date and time for issues reported during working hours. It calculates the due date based on a given submission timestamp and turnaround time (in working hours). The project adheres strictly to business rules regarding working hours, non-working days, and turnaround times.

## Features

- **Precise Due Date Calculation**: Calculates the due date based on submission time and turnaround hours.
- **Business Rule Adherence**: Accounts for working hours (9 AM - 5 PM) and excludes weekends.
- **Error Handling**: Validates input data (e.g., negative turnaround times) and throws appropriate exceptions.
- **Clean Code Standards**: Written with industry-standard practices for readability and maintainability.
- **Automated Testing**: Includes unit tests to ensure correctness of calculations.
- **Extensible Design**: Modular architecture for easy enhancement.

## Technologies

- **Programming Language**: C# (.NET)
- **Testing Framework**: xUnit
- **Development Environment**: Visual Studio or any compatible IDE
- **Version Control**: Git

## Project Structure

The repository is organized as follows:

```
text
src/
├── DueDateCalculator/
│   ├── Models/
│   │   └── SubmitInfo.cs         # Model representing submission details
│   ├── Services/
│   │   └── DueDateCalculatorService.cs # Core service for due date calculation
│   └── Program.cs                # Entry point of the application
├── DueDateCalculator.Tests/
│   └── DueDateCalculatorTests.cs # Unit tests for the service
```

## Getting Started

Follow these steps to set up and run the project locally.

## Prerequisites

Ensure you have the following installed:

1. **.NET SDK** (version 6.0 or later)
2. **Visual Studio 2022** or any compatible IDE
3. **Git** (for version control)

## Installation

1. Clone the repository:

   ```
   bash
   git clone https://github.com/yourusername/due-date-calculator.git
   cd due-date-calculator
   ```

2. Build the project:

   ```
   bash
   dotnet build
   ```

3. Run tests:

   ```
   bash
   dotnet test
   ```

## Implementation Details

## Business Rules

1. **Working Hours**:
   - Monday to Friday.
   - From 9 AM to 5 PM.
2. **Turnaround Time**:
   - Defined in working hours (e.g., 16 hours = 2 days).
3. **Submission Time Constraints**:
   - Issues can only be submitted during working hours.

## Core Functionality

The `DueDateCalculatorService` class implements the core logic for calculating due dates based on submission details (`SubmitInfo`). Key methods include:

1. `CalculateDueDate`: Public method that validates input and calculates the due date.
2. `IsWorkingDay`: Determines if a given date falls on a working day.
3. `GetWorkingHoursInDay`: Calculates remaining working hours in a day based on submission time.
4. `MoveToNextWorkingDay`: Moves to the next valid working day.
5. `AdjustDueDate`: Adjusts the due date based on remaining turnaround hours.

## Testing

## Test Coverage

The project includes unit tests using xUnit framework to validate all major functionalities of the `DueDateCalculatorService`. Key test cases include:

1. Submission on Monday with a turnaround of 16 hours results in Wednesday's due date.
2. Submission on Friday afternoon with a turnaround of 16 hours results in Tuesday's due date.
3. Submission at exactly 9 AM calculates correctly.
4. Zero turnaround time returns the same submission time as due date.
5. Negative turnaround time throws an exception.

Run tests using:

```
bash
dotnet test
```

## Troubleshooting

## Common Issues

1. **Build Errors**:
   - Ensure .NET SDK is installed and compatible with the project version.
2. **Test Failures**:
   - Verify that system time zones match expectations (tests assume UTC).

## Debugging Tips

1. Use Visual Studio's debugger to step through methods in `DueDateCalculatorService`.
2. Check input values thoroughly before invoking `CalculateDueDate`.

## Development Notes

## Known Issues

1. The solution assumes holidays are treated as regular working days, which may not align with all real-world scenarios.
2. Currently, non-working minutes (e.g., lunch breaks) are not accounted for.

## Improvements and Future Enhancements

1. Add support for configurable holidays.
2. Extend functionality to handle different time zones.
3. Implement additional logging for debugging purposes.
4. Optimize performance for large-scale systems.

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork this repository.
2. Create a feature branch (`git checkout -b feature-name`).
3. Commit your changes (`git commit -m "Add feature"`).
4. Push your branch (`git push origin feature-name`).
5. Open a pull request.

## License

This project is licensed under the MIT License.

## Contact Information

For questions or feedback, feel free to reach out:

- Email: [cc.kprohaszka](mailto:cc.krohaszka@gmail.com)
- GitHub: [kprohaszka](https://github.com/kprohaszka)

Thank you for exploring the **Due Date Calculator**!