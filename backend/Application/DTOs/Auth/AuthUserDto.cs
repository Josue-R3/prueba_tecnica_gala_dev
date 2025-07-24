namespace Application.DTOs.Auth;

public record AuthUserDto
{
    public int Id { get; init; }
    public required string Usuario { get; init; }
    public int RolId { get; init; }
    public int? EmpleadoId { get; init; }
    public RolDto Rol { get; init; } = null!;
    public EmpleadoDto? Empleado { get; init; }
}

public record RolDto
{
    public int Id { get; init; }
    public required string Nombre { get; init; }
}

public record EmpleadoDto
{
    public int Id { get; init; }
    public required string Nombre { get; init; }
    public required string Apellido { get; init; }
    public required string Cargo { get; init; }
}
