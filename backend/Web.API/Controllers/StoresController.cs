using Application.DTOs.Store;
using Application.DTOs.Common;
using Application.Interfaces;
using Application.Validators.Store;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoresController : ControllerBase
{
    private readonly IStoreService _storeService;
    private readonly CreateStoreDtoValidator _createValidator;
    private readonly UpdateStoreDtoValidator _updateValidator;

    public StoresController(
        IStoreService storeService,
        CreateStoreDtoValidator createValidator,
        UpdateStoreDtoValidator updateValidator)
    {
        _storeService = storeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get all stores
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StoreDto>>> GetAllStores()
    {
        var stores = await _storeService.GetAllAsync();
        return Ok(stores);
    }

    /// <summary>
    /// Get stores with pagination
    /// </summary>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResultDto<StoreDto>>> GetStoresPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var result = await _storeService.GetPagedAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Get store by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<StoreDto>> GetStore(int id)
    {
        var store = await _storeService.GetByIdAsync(id);
        if (store == null)
            return NotFound($"Store with ID {id} not found");

        return Ok(store);
    }

    /// <summary>
    /// Create a new store
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<StoreDto>> CreateStore(CreateStoreDto createStoreDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createStoreDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var store = await _storeService.CreateAsync(createStoreDto);
        return CreatedAtAction(nameof(GetStore), new { id = store.Id }, store);
    }

    /// <summary>
    /// Update an existing store
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<StoreDto>> UpdateStore(int id, UpdateStoreDto updateStoreDto)
    {
        if (id != updateStoreDto.Id)
            return BadRequest("ID mismatch");

        var validationResult = await _updateValidator.ValidateAsync(updateStoreDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var store = await _storeService.UpdateAsync(updateStoreDto);
            return Ok(store);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Delete a store
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStore(int id)
    {
        var result = await _storeService.DeleteAsync(id);
        if (!result)
            return NotFound($"Store with ID {id} not found");

        return NoContent();
    }
}
