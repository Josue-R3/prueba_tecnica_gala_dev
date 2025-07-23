using Application.DTOs.Common;
using Application.DTOs.Employee;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        return employee != null ? _mapper.Map<EmployeeDto>(employee) : null;
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto createEmployeeDto)
    {
        var employee = _mapper.Map<Employee>(createEmployeeDto);
        var createdEmployee = await _unitOfWork.Employees.CreateAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<EmployeeDto>(createdEmployee);
    }

    public async Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto updateEmployeeDto)
    {
        var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(updateEmployeeDto.Id);
        if (existingEmployee == null)
            throw new ArgumentException($"Employee with ID {updateEmployeeDto.Id} not found");

        _mapper.Map(updateEmployeeDto, existingEmployee);
        var updatedEmployee = await _unitOfWork.Employees.UpdateAsync(existingEmployee);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<EmployeeDto>(updatedEmployee);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _unitOfWork.Employees.DeleteAsync(id);
        if (result)
            await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<IEnumerable<EmployeeDto>> SearchAsync(string searchTerm)
    {
        var employees = await _unitOfWork.Employees.SearchAsync(searchTerm);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<PagedResultDto<EmployeeDto>> GetPagedAsync(int page, int pageSize, string? searchTerm = null)
    {
        var (items, totalCount) = await _unitOfWork.Employees.GetPagedAsync(page, pageSize, searchTerm);
        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(items);

        return new PagedResultDto<EmployeeDto>
        {
            Items = employeeDtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}
