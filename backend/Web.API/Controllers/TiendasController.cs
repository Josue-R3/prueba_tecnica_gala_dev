using Application.DTOs.Tienda;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TiendasController : ControllerBase
{
    private readonly ITiendaService _tiendaService;
    private readonly ILogger<TiendasController> _logger;

    public TiendasController(ITiendaService tiendaService, ILogger<TiendasController> logger)
    {
        _tiendaService = tiendaService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las tiendas
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TiendaDto>>> GetAll()
    {
        try
        {
            var tiendas = await _tiendaService.GetAllAsync();
            return Ok(tiendas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las tiendas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene solo las tiendas activas
    /// </summary>
    [HttpGet("activas")]
    public async Task<ActionResult<IEnumerable<TiendaDto>>> GetActivas()
    {
        try
        {
            var tiendas = await _tiendaService.GetActivasAsync();
            return Ok(tiendas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tiendas activas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una tienda por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TiendaDto>> GetById(int id)
    {
        try
        {
            var tienda = await _tiendaService.GetByIdAsync(id);
            if (tienda == null)
                return NotFound($"No se encontró la tienda con ID {id}");

            return Ok(tienda);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tienda con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea una nueva tienda
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TiendaDto>> Create([FromBody] CreateTiendaDto createDto)
    {
        try
        {
            var tienda = await _tiendaService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = tienda.Id }, tienda);
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
            _logger.LogError(ex, "Error al crear tienda");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza una tienda existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TiendaDto>> Update(int id, [FromBody] UpdateTiendaDto updateDto)
    {
        try
        {
            if (id != updateDto.Id)
                return BadRequest("El ID del parámetro no coincide con el ID del cuerpo de la petición");

            var tienda = await _tiendaService.UpdateAsync(updateDto);
            return Ok(tienda);
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
            _logger.LogError(ex, "Error al actualizar tienda con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina (desactiva) una tienda
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var result = await _tiendaService.DeleteAsync(id);
            if (!result)
                return NotFound($"No se encontró la tienda con ID {id}");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar tienda con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
