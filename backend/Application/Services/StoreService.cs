using Application.DTOs.Common;
using Application.DTOs.Store;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class StoreService : IStoreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StoreService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StoreDto>> GetAllAsync()
    {
        var stores = await _unitOfWork.Stores.GetAllAsync();
        return _mapper.Map<IEnumerable<StoreDto>>(stores);
    }

    public async Task<StoreDto?> GetByIdAsync(int id)
    {
        var store = await _unitOfWork.Stores.GetByIdAsync(id);
        return store != null ? _mapper.Map<StoreDto>(store) : null;
    }

    public async Task<StoreDto> CreateAsync(CreateStoreDto createStoreDto)
    {
        var store = _mapper.Map<Store>(createStoreDto);
        var createdStore = await _unitOfWork.Stores.CreateAsync(store);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StoreDto>(createdStore);
    }

    public async Task<StoreDto> UpdateAsync(UpdateStoreDto updateStoreDto)
    {
        var existingStore = await _unitOfWork.Stores.GetByIdAsync(updateStoreDto.Id);
        if (existingStore == null)
            throw new ArgumentException($"Store with ID {updateStoreDto.Id} not found");

        _mapper.Map(updateStoreDto, existingStore);
        var updatedStore = await _unitOfWork.Stores.UpdateAsync(existingStore);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StoreDto>(updatedStore);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _unitOfWork.Stores.DeleteAsync(id);
        if (result)
            await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<PagedResultDto<StoreDto>> GetPagedAsync(int page, int pageSize)
    {
        var (items, totalCount) = await _unitOfWork.Stores.GetPagedAsync(page, pageSize);
        var storeDtos = _mapper.Map<IEnumerable<StoreDto>>(items);

        return new PagedResultDto<StoreDto>
        {
            Items = storeDtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}
