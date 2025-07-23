using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public EmployeeStatus Status { get; set; }

    // Navigation properties
    public string FullName => $"{FirstName} {LastName}";
}
