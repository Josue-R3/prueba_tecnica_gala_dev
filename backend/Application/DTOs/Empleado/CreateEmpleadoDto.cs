namespace Application.DTOs.Empleado;

public class CreateEmpleadoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public DateTime FechaIngreso { get; set; } = DateTime.Now;
    public int TiendaId { get; set; }
}
