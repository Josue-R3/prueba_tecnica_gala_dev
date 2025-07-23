using Application.DTOs.Common;
using Application.DTOs.Store;

namespace Application.Interfaces;

public interface IStoreService
{
    Task<IEnumerable<StoreDto>> GetAllAsync();
    Task<StoreDto?> GetByIdAsync(int id);
    Task<StoreDto> CreateAsync(CreateStoreDto createStoreDto);
    Task<StoreDto> UpdateAsync(UpdateStoreDto updateStoreDto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<StoreDto>> GetPagedAsync(int page, int pageSize);
}
