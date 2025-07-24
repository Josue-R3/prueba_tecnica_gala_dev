namespace Domain.Entities;

public class Tienda
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
