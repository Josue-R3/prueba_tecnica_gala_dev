using Domain.Entities;

namespace Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByUsuarioNameAsync(string usuarioName);
    Task<bool> ExistsByUsuarioNameAsync(string usuarioName, int? excludeId = null);
    Task<Usuario?> GetByUsernameAsync(string username);
}
