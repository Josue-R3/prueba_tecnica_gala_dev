using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TiendaRepository : Repository<Tienda>, ITiendaRepository
{
    public TiendaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Tienda?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(t => t.Empleados.Where(e => e.Estado))
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public override async Task<IEnumerable<Tienda>> GetAllAsync()
    {
        return await _dbSet
            .Include(t => t.Empleados.Where(e => e.Estado))
            .OrderBy(t => t.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tienda>> GetTiendasActivasAsync()
    {
        return await _dbSet
            .Include(t => t.Empleados.Where(e => e.Estado))
            .Where(t => t.Estado)
            .OrderBy(t => t.Nombre)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null)
    {
        var query = _dbSet.Where(t => t.Nombre.ToLower() == nombre.ToLower());

        if (excludeId.HasValue)
            query = query.Where(t => t.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}
