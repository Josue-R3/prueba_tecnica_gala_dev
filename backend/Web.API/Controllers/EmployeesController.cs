using Application.DTOs.Employee;
using Application.DTOs.Common;
using Application.Interfaces;
using Application.Validators.Employee;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly CreateEmployeeDtoValidator _createValidator;
    private readonly UpdateEmployeeDtoValidator _updateValidator;

    public EmployeesController(
        IEmployeeService employeeService,
        CreateEmployeeDtoValidator createValidator,
        UpdateEmployeeDtoValidator updateValidator)
    {
        _employeeService = employeeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }

    /// <summary>
    /// Get employees with pagination
    /// </summary>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResultDto<EmployeeDto>>> GetEmployeesPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var result = await _employeeService.GetPagedAsync(page, pageSize, searchTerm);
        return Ok(result);
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null)
            return NotFound($"Employee with ID {id} not found");

        return Ok(employee);
    }

    /// <summary>
    /// Search employees by name or email
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> SearchEmployees([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("Search term is required");

        var employees = await _employeeService.SearchAsync(searchTerm);
        return Ok(employees);
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto createEmployeeDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createEmployeeDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var employee = await _employeeService.CreateAsync(createEmployeeDto);
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
    }

    /// <summary>
    /// Update an existing employee
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
    {
        if (id != updateEmployeeDto.Id)
            return BadRequest("ID mismatch");

        var validationResult = await _updateValidator.ValidateAsync(updateEmployeeDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var employee = await _employeeService.UpdateAsync(updateEmployeeDto);
            return Ok(employee);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Delete an employee
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        var result = await _employeeService.DeleteAsync(id);
        if (!result)
            return NotFound($"Employee with ID {id} not found");

        return NoContent();
    }
}
