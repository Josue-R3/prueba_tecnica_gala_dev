using Domain.Entities;

namespace Domain.Interfaces;

public interface ITiendaRepository : IRepository<Tienda>
{
    Task<IEnumerable<Tienda>> GetTiendasActivasAsync();
    Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null);
}
