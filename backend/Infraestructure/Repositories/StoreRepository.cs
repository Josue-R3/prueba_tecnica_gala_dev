using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly ApplicationDbContext _context;

    public StoreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Store>> GetAllAsync()
    {
        return await _context.Stores
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Store?> GetByIdAsync(int id)
    {
        return await _context.Stores.FindAsync(id);
    }

    public async Task<Store> CreateAsync(Store store)
    {
        _context.Stores.Add(store);
        return store;
    }

    public async Task<Store> UpdateAsync(Store store)
    {
        _context.Stores.Update(store);
        return store;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var store = await _context.Stores.FindAsync(id);
        if (store == null)
            return false;

        _context.Stores.Remove(store);
        return true;
    }

    public async Task<(IEnumerable<Store> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var totalCount = await _context.Stores.CountAsync();

        var items = await _context.Stores
            .OrderBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
