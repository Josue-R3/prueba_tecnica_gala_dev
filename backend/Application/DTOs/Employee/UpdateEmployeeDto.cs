using Domain.Enums;

namespace Application.DTOs.Employee;

public record UpdateEmployeeDto
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Position { get; init; } = string.Empty;
    public DateTime HireDate { get; init; }
    public EmployeeStatus Status { get; init; }
}
