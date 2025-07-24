using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        // Filtrar solo registros activos (Estado = true)
        var entityType = typeof(T);
        var estadoProperty = entityType.GetProperty("Estado");

        if (estadoProperty != null && estadoProperty.PropertyType == typeof(bool))
        {
            // Si la entidad tiene propiedad Estado, filtrar solo activos
            return await _dbSet.Where(e => EF.Property<bool>(e, "Estado") == true).ToListAsync();
        }
        else
        {
            // Si no tiene Estado, devolver todos
            return await _dbSet.ToListAsync();
        }
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.FromResult(entity);
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
            return false;

        // Soft Delete: Cambiar estado a false en lugar de eliminación física
        var entityType = typeof(T);
        var estadoProperty = entityType.GetProperty("Estado");

        if (estadoProperty != null && estadoProperty.PropertyType == typeof(bool))
        {
            // Si la entidad tiene propiedad Estado, hacer soft delete
            estadoProperty.SetValue(entity, false);
            _dbSet.Update(entity);
        }
        else
        {
            // Si no tiene Estado, eliminación física (fallback)
            _dbSet.Remove(entity);
        }

        return true;
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.FindAsync(id) != null;
    }
}
