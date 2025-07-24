namespace Domain.Entities;

public class Rol
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
