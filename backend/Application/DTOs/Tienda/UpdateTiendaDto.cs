namespace Application.DTOs.Tienda;

public class UpdateTiendaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public bool Estado { get; set; }
}
