namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEmpleadoRepository EmpleadoRepository { get; }
    ITiendaRepository TiendaRepository { get; }
    IUsuarioRepository UsuarioRepository { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
