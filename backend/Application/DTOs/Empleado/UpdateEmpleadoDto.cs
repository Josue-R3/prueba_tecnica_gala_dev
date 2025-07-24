namespace Application.DTOs.Empleado;

public class UpdateEmpleadoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public DateTime FechaIngreso { get; set; }
    public bool Estado { get; set; }
    public int TiendaId { get; set; }
}
