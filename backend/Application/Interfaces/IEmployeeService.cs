using Application.DTOs.Common;
using Application.DTOs.Employee;

namespace Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto createEmployeeDto);
    Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto updateEmployeeDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<EmployeeDto>> SearchAsync(string searchTerm);
    Task<PagedResultDto<EmployeeDto>> GetPagedAsync(int page, int pageSize, string? searchTerm = null);
}
