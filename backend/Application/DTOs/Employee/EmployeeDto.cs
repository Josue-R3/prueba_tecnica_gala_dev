using Domain.Enums;

namespace Application.DTOs.Employee;

public record EmployeeDto
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Position { get; init; } = string.Empty;
    public DateTime HireDate { get; init; }
    public EmployeeStatus Status { get; init; }
    public string FullName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
