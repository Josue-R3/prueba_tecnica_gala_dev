using Application.DTOs.Common;
using Application.DTOs.Empleado;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmpleadosController : ControllerBase
{
    private readonly IEmpleadoService _empleadoService;
    private readonly ILogger<EmpleadosController> _logger;

    public EmpleadosController(IEmpleadoService empleadoService, ILogger<EmpleadosController> logger)
    {
        _empleadoService = empleadoService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los empleados activos
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetAll()
    {
        try
        {
            var empleados = await _empleadoService.GetAllAsync();
            return Ok(empleados);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los empleados");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene empleados con paginación y búsqueda opcional
    /// </summary>
    [HttpGet("paginated")]
    public async Task<ActionResult<PagedResultDto<EmpleadoDto>>> GetPaginated(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var result = await _empleadoService.GetPaginatedAsync(pageNumber, pageSize, searchTerm);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener empleados paginados");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un empleado por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmpleadoDto>> GetById(int id)
    {
        try
        {
            var empleado = await _empleadoService.GetByIdAsync(id);
            if (empleado == null)
                return NotFound($"No se encontró el empleado con ID {id}");

            return Ok(empleado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener empleado con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Busca empleados por nombre o correo
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<EmpleadoDto>>> Search([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("El término de búsqueda no puede estar vacío");

            var empleados = await _empleadoService.SearchAsync(searchTerm);
            return Ok(empleados);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar empleados con término: {SearchTerm}", searchTerm);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo empleado
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EmpleadoDto>> Create([FromBody] CreateEmpleadoDto createDto)
    {
        try
        {
            var empleado = await _empleadoService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = empleado.Id }, empleado);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage });
            return BadRequest(new { Message = "Errores de validación", Errors = errors });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear empleado");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un empleado existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<EmpleadoDto>> Update(int id, [FromBody] UpdateEmpleadoDto updateDto)
    {
        try
        {
            if (id != updateDto.Id)
                return BadRequest("El ID del parámetro no coincide con el ID del cuerpo de la petición");

            var empleado = await _empleadoService.UpdateAsync(updateDto);
            return Ok(empleado);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage });
            return BadRequest(new { Message = "Errores de validación", Errors = errors });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar empleado con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina (desactiva) un empleado
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var result = await _empleadoService.DeleteAsync(id);
            if (!result)
                return NotFound($"No se encontró el empleado con ID {id}");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar empleado con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
