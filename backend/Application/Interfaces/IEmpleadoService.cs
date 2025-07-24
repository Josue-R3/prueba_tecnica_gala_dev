using Application.DTOs.Common;
using Application.DTOs.Empleado;

namespace Application.Interfaces;

public interface IEmpleadoService
{
    Task<EmpleadoDto?> GetByIdAsync(int id);
    Task<IEnumerable<EmpleadoDto>> GetAllAsync();
    Task<PagedResultDto<EmpleadoDto>> GetPaginatedAsync(int pageNumber, int pageSize, string? searchTerm = null);
    Task<IEnumerable<EmpleadoDto>> SearchAsync(string searchTerm);
    Task<EmpleadoDto> CreateAsync(CreateEmpleadoDto createDto);
    Task<EmpleadoDto> UpdateAsync(UpdateEmpleadoDto updateDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
