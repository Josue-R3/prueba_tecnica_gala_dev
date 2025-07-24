using Domain.Entities;

namespace Domain.Interfaces;

public interface IEmpleadoRepository : IRepository<Empleado>
{
    Task<IEnumerable<Empleado>> GetEmpleadosByTiendaAsync(int tiendaId);
    Task<IEnumerable<Empleado>> SearchEmpleadosAsync(string searchTerm);
    Task<(IEnumerable<Empleado> empleados, int totalCount)> GetEmpleadosPaginatedAsync(int pageNumber, int pageSize, string? searchTerm = null);
    Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null);
}
