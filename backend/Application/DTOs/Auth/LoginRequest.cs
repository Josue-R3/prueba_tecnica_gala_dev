using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

public record LoginRequest
{
    [Required(ErrorMessage = "El usuario es requerido")]
    [StringLength(50, ErrorMessage = "El usuario no puede exceder 50 caracteres")]
    public required string Usuario { get; init; }

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(255, ErrorMessage = "La contraseña no puede exceder 255 caracteres")]
    public required string Contrasenia { get; init; }
}
