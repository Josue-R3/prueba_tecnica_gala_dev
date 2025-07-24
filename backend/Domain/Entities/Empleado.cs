namespace Domain.Entities;

public class Empleado
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public DateTime FechaIngreso { get; set; } = DateTime.Now;
    public bool Estado { get; set; } = true;
    public int TiendaId { get; set; }

    // Navigation properties
    public virtual Tienda Tienda { get; set; } = null!;
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
