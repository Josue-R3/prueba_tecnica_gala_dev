using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmpleadoRepository : Repository<Empleado>, IEmpleadoRepository
{
    public EmpleadoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Empleado?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(e => e.Tienda)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public override async Task<IEnumerable<Empleado>> GetAllAsync()
    {
        return await _dbSet
            .Include(e => e.Tienda)
            .Where(e => e.Estado)
            .OrderBy(e => e.Apellido)
            .ThenBy(e => e.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Empleado>> GetEmpleadosByTiendaAsync(int tiendaId)
    {
        return await _dbSet
            .Include(e => e.Tienda)
            .Where(e => e.TiendaId == tiendaId && e.Estado)
            .OrderBy(e => e.Apellido)
            .ThenBy(e => e.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Empleado>> SearchEmpleadosAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();

        var lowerSearchTerm = searchTerm.ToLower();

        return await _dbSet
            .Include(e => e.Tienda)
            .Where(e => e.Estado &&
                       (e.Nombre.ToLower().Contains(lowerSearchTerm) ||
                        e.Apellido.ToLower().Contains(lowerSearchTerm) ||
                        e.Correo.ToLower().Contains(lowerSearchTerm)))
            .OrderBy(e => e.Apellido)
            .ThenBy(e => e.Nombre)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Empleado> empleados, int totalCount)> GetEmpleadosPaginatedAsync(
        int pageNumber, int pageSize, string? searchTerm = null)
    {
        var query = _dbSet
            .Include(e => e.Tienda)
            .Where(e => e.Estado);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerSearchTerm = searchTerm.ToLower();
            query = query.Where(e =>
                e.Nombre.ToLower().Contains(lowerSearchTerm) ||
                e.Apellido.ToLower().Contains(lowerSearchTerm) ||
                e.Correo.ToLower().Contains(lowerSearchTerm));
        }

        var totalCount = await query.CountAsync();

        var empleados = await query
            .OrderBy(e => e.Apellido)
            .ThenBy(e => e.Nombre)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (empleados, totalCount);
    }

    public async Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null)
    {
        var query = _dbSet.Where(e => e.Correo.ToLower() == correo.ToLower());

        if (excludeId.HasValue)
            query = query.Where(e => e.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}
