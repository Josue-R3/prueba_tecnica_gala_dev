using Application.DTOs.Tienda;

namespace Application.Interfaces;

public interface ITiendaService
{
    Task<TiendaDto?> GetByIdAsync(int id);
    Task<IEnumerable<TiendaDto>> GetAllAsync();
    Task<IEnumerable<TiendaDto>> GetActivasAsync();
    Task<TiendaDto> CreateAsync(CreateTiendaDto createDto);
    Task<TiendaDto> UpdateAsync(UpdateTiendaDto updateDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
