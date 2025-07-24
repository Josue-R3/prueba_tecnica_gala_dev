using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(u => u.Rol)
            .Include(u => u.Empleado)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public override async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _dbSet
            .Include(u => u.Rol)
            .Include(u => u.Empleado)
            .Where(u => u.Estado)
            .OrderBy(u => u.UsuarioName)
            .ToListAsync();
    }

    public async Task<Usuario?> GetByUsuarioNameAsync(string usuarioName)
    {
        return await _dbSet
            .Include(u => u.Rol)
            .Include(u => u.Empleado)
            .FirstOrDefaultAsync(u => u.UsuarioName.ToLower() == usuarioName.ToLower() && u.Estado);
    }

    public async Task<bool> ExistsByUsuarioNameAsync(string usuarioName, int? excludeId = null)
    {
        var query = _dbSet.Where(u => u.UsuarioName.ToLower() == usuarioName.ToLower());

        if (excludeId.HasValue)
            query = query.Where(u => u.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}
