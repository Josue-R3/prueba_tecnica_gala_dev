using Domain.Entities;

namespace Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> CreateAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Employee>> SearchAsync(string searchTerm);
    Task<(IEnumerable<Employee> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? searchTerm = null);
}
