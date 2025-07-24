namespace Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string UsuarioName { get; set; } = string.Empty;
    public string Contrasenia { get; set; } = string.Empty;
    public byte RolId { get; set; }
    public int? EmpleadoId { get; set; }
    public bool Estado { get; set; } = true;
    public DateTime FechaCreado { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual Rol Rol { get; set; } = null!;
    public virtual Empleado? Empleado { get; set; }
}
